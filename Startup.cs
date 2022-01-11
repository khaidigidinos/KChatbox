using System;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using SignalRApi.DatabaseContexts;
using SignalRApi.Filters;
using SignalRApi.Hubs;
using SignalRApi.Repositories;
using SignalRApi.Services;

namespace SignalRApi
{
    public class Startup
    {
        private readonly IConfiguration Configuration;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers(opt => {
                /* Add more filters here */
                opt.Filters.Add<ApiExceptionFilter>();
            });

            services.AddSingleton<IUserIdProvider, CustomUserIdProvider>();

            services.AddCors(opt =>
            {
                opt.AddPolicy("signalrClient", builder => {
                    builder
                        .WithOrigins("https://hidden-river-60444.herokuapp.com", "http://localhost:3000")
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                });
            });

            services.AddHttpContextAccessor();

            var userDatabaseConnectionString = Configuration.GetValue<string>("ConnectionStrings:UserConnectionString");

            services.AddDbContext<UserDatabaseContext>(opts =>
            {
                opts.UseMySql(userDatabaseConnectionString, ServerVersion.AutoDetect(userDatabaseConnectionString));
            });

            services.AddScoped<IUserDatabaseContext>(sp => sp.GetRequiredService<UserDatabaseContext>());

            var jwtOptions = Configuration.GetSection("JwtConfig");

            services.AddAuthentication(opt => {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, opts => {
                opts.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateIssuerSigningKey = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidIssuer = jwtOptions.GetValue<string>("Issuer"),
                    ValidAudiences = new string[] { jwtOptions.GetValue<string>("Audience") },
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.GetValue<string>("Secret")))
                };

                opts.Events = new JwtBearerEvents()
                {
                    OnMessageReceived = context =>
                    {
                        var token = context.Request.Query["access_token"];
                        var path = context.Request.Path;
                        if (!string.IsNullOrEmpty(token) && path.StartsWithSegments("/hubs"))
                        {
                            context.Token = token;
                        }
                        return Task.CompletedTask;
                    }
                };
                opts.Validate();
            });

            services.AddSignalR();

            services.AddAuthorization(opt =>
            {
                // Add more policies here. Mainly used for AuthorizeAttribute
            });

            services.AddFluentValidation(opt => {
                opt.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            });

            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddPipelines();

            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            services.AddScoped<IRoomDatabaseContext, RoomDatabaseContext>();

            /* WARNING: Based on the scope, define type of DI properly */
            services.AddSingleton<JwtService>();

            services.AddScoped<UserService>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<CurrentUserService>();
            services.AddScoped<RoomService>();
            services.AddScoped<IRoomRepository, RoomRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseCors("signalrClient");

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<RoomChatHub>("/hubs/roomchat");
                endpoints.MapControllers();
            });
        }
    }
}
