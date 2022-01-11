using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SignalRApi.DatabaseContexts;
using SignalRApi.Migrations.RoomDatabase;
using SignalRApi.Migrations.UserDatabase;

namespace SignalRApi
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<IUserDatabaseContext>();
                var roomContext = scope.ServiceProvider.GetRequiredService<IRoomDatabaseContext>();

                await UserDatabaseSeeder.InitRoleData(dbContext);
                await RoomDatabaseSeeder.InitUserData(roomContext, dbContext);
            }

            await host.RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
