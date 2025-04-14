using Microsoft.IdentityModel.Tokens;
using Module_5.Collections;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Module_5.Utilities
{
    public class JwtTokenHelper
    {
        private readonly IConfiguration _config;

        public JwtTokenHelper(IConfiguration config)
        {
            _config = config;
        }
        public string GenerateJwtToken(string userId, string name, string email, UserRole role, int tokenVersion)
        {

            var jwtSettings = _config.GetSection("Jwt");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            string roleStr = Convert.ToString(role) ?? "";

            var claims = new List<Claim>
                {   new(ClaimTypes.NameIdentifier, userId),
                    new(ClaimTypes.Name, name),
                    new(ClaimTypes.Email, email),
                    new(ClaimTypes.Role, roleStr),
                    new("TokenVersion", tokenVersion.ToString())
                };

            var token = new JwtSecurityToken(
                    issuer: jwtSettings["Issuer"],
                    audience: jwtSettings["Audience"],
                    claims: claims,
                    expires: DateTime.UtcNow.AddMinutes(double.Parse(jwtSettings["ExpireMinutes"])),
                    signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
