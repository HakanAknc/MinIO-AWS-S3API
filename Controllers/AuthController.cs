using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using S3AdvancedV2.Models;
using S3AdvancedV2.Services;

namespace S3AdvancedV2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;
        private readonly UserService _userService;

        
        public AuthController(AuthService authService, UserService userService)
        {
            _authService = authService;
            _userService = userService;
        }

        // POST: api/auth/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] S3AdvancedV2.Models.LoginRequest request)
        {
            var token = await _authService.LoginAsync(request.Username, request.Password);
            if (token == null) return Unauthorized();

            return Ok(new { token });
        }

        // POST: api/auth/register
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserModel user)
        {
            await _userService.CreateAsync(user);
            return Ok("Kullanıcı oluşturuldu");
        }

        // GET: api/auth/users
        [HttpGet("users")]
        public async Task<IActionResult> Users()
        {
            var users = await _userService.GetAllAsync();
            return Ok(users.Select(u => new { u.Username, u.Role }));
        }
    }
}
