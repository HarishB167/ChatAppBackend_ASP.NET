using System.Configuration;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;
using System.Collections.Generic;

namespace ChatAppBackend.Utils
{
    public class Utils
    {
        public static string GenerateJwtToken(object data)
        {
            var secretKey = Encoding.UTF8.GetBytes(ConfigurationManager.AppSettings["JwtKey"]);
            var signingKey = new SymmetricSecurityKey(secretKey);
            
            var claims = new List<Claim>();

            foreach (PropertyInfo prop in data.GetType().GetProperties())
            {
                claims.Add(new Claim(prop.Name, prop.GetValue(data, null).ToString()));

            }

            var token = new JwtSecurityToken(
                issuer: ConfigurationManager.AppSettings["JwtIssuer"],
                audience: ConfigurationManager.AppSettings["JwtAudience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(12), // Set the token expiration
                signingCredentials: new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public static bool ValidateJwtToken(string token)
        {
            var secretKey = Encoding.UTF8.GetBytes(ConfigurationManager.AppSettings["JwtKey"]);
            var signingKey = new SymmetricSecurityKey(secretKey);

            var tokenHandler = new JwtSecurityTokenHandler();
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = signingKey,
                ValidateIssuer = true,
                ValidIssuer = ConfigurationManager.AppSettings["JwtIssuer"],
                ValidateAudience = true,
                ValidAudience = ConfigurationManager.AppSettings["JwtAudience"],
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero // No tolerance for time difference
            };

            try
            {
                SecurityToken validatedToken;
                tokenHandler.ValidateToken(token, validationParameters, out validatedToken);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}