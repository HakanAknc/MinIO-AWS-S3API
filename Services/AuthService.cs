using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace S3AdvancedV2.Services
{
    public class AuthService
    {
        private readonly IConfiguration _config;
        private readonly UserService _userService;

        public AuthService(IConfiguration config, UserService userService)
        {
            _config = config;
            _userService = userService;
        }

        // token oluşturma ve kullanıcı doğrulama işlemleri
        public async Task<string> LoginAsync(string username, string password)
        {
            var user = await _userService.GetByUsernameAsync(username);
            if (user == null || user.Password != password) return null;

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
