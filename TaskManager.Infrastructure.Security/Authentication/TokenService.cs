using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Core.Application.Interfaces.Authentication;
using TaskManager.Core.Domain.Entities;

namespace TaskManager.Infrastructure.Security.Authentication
{
    public class TokenService : ITokenService
    {
        private readonly string _secretKey;
        private readonly string _audience;
        private readonly string _issuer;

        public TokenService(IConfiguration config)
        {
            _secretKey = config.GetSection("Authentication:SecretKey").Value!;
            _audience = config.GetSection("Authentication:Audience").Value!;
            _issuer = config.GetSection("Authentication:Issuer").Value!;
        }

        public string GenerateToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_secretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.Name),
                    new Claim(ClaimTypes.Surname, user.LastName),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.GivenName, user.UserName)
                }),
                Audience = _audience,
                Issuer = _issuer,
                Expires = DateTime.UtcNow.AddHours(3),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
