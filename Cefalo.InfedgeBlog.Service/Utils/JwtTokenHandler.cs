using Cefalo.InfedgeBlog.Database.Models;
using Cefalo.InfedgeBlog.Service.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Cefalo.InfedgeBlog.Service.Utils
{
    public class JwtTokenHandler : IJwtTokenHandler
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public JwtTokenHandler(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }

        public string GenerateJwtToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Expiration, DateTime.UtcNow.ToString())
            };
            var configKey = _configuration.GetSection("AppSettings:Token").Value;
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configKey));
            var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddDays(3),  
                signingCredentials: signingCredentials
            );
            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
            return tokenString;
        }
        public int GetLoggedInUserId()
        {
            var Id = -1;
            if (_httpContextAccessor.HttpContext != null)
            {
                Id = Int32.Parse(_httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            }
            return Id;
        }
        public Boolean IsTokenExpired()
        {
            var tokenGenerationTimeString = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Expiration).Value;
            if(tokenGenerationTimeString == null)
            {
                return true;
            }
            var tokenGenerationTime = Convert.ToDateTime(tokenGenerationTimeString);
            DateTime expirationTime = tokenGenerationTime.AddDays(3);
            DateTime currentTime = DateTime.UtcNow;
            return currentTime > expirationTime;
        }
        public void DeleteToken()
        {
            _httpContextAccessor.HttpContext = null;
        }
    }
}
