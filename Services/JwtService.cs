using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace SignalRApi.Services
{
    public class JwtService
    {
        private readonly IConfiguration _configuration;

        public JwtService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string CreateJwtToken(string id, string username, string useremail, string[] roles, string clientId = null)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtConfig:Secret"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claimsList = new List<Claim>()
            {
                new Claim("id", id),
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Email, useremail),
            };

            foreach (var role in roles)
            {
                claimsList.Add(new Claim(ClaimTypes.Role, role));
            }

            var token = new JwtSecurityToken(_configuration["JwtConfig:Issuer"],
              clientId ?? _configuration["JwtConfig:Audience"],
              claimsList,
              expires: DateTime.Now.AddMinutes(_configuration.GetValue<int>("JwtConfig:LifeTime")),
              signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public bool ValidateToken(string token, out ClaimsPrincipal claimsPrincipal)
        {
            var jwtOptions = _configuration.GetSection("JwtConfig");

            var tokenValidate = new TokenValidationParameters()
            {
                ValidateIssuer = true,
                ValidateIssuerSigningKey = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidIssuer = jwtOptions.GetValue<string>("Issuer"),
                ValidAudiences = new string[] { jwtOptions.GetValue<string>("Audience") },
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.GetValue<string>("Secret")))
            };
            var handler = new JwtSecurityTokenHandler();
            claimsPrincipal = handler.ValidateToken(token, tokenValidate, out var _);
            if (claimsPrincipal is not null && claimsPrincipal.Claims.Count() > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
