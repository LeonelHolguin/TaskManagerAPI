using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace WebAPI.TaskManager.Configuration
{
    public static class JwtAuthenticationConfiguration
    {
        public static void AddJwtAuthentication(this IServiceCollection services, IConfiguration config)
        {
            const string defaultScheme = JwtBearerDefaults.AuthenticationScheme;
            var secretKey = config.GetSection("Authentication:SecretKey").Value!;
            var validAudience = config.GetSection("Authentication:Audience").Value!;
            var validIssuer = config.GetSection("Authentication:Issuer").Value!;

            var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretKey));

            services.AddAuthentication(config =>
            {
                config.DefaultAuthenticateScheme = defaultScheme;
                config.DefaultChallengeScheme = defaultScheme;

            }).AddJwtBearer(config =>
            {
                config.RequireHttpsMetadata = false;
                config.SaveToken = true;
                config.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = signingKey,
                    ValidateIssuer = true,
                    ValidIssuer = validIssuer,
                    ValidateAudience = true,
                    ValidAudience = validAudience,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.FromSeconds(30)
                };
            });
        }
    }
}
