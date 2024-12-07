using Microsoft.AspNetCore.Mvc;
using P2.Models;
using P2.Services;
using System.Security.Claims;
using System.Text;

namespace P2.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController()
        {
            _authService = new AuthService();
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] User user)
        {
            var validUser = _authService.ValidateUser(user.UserName, user.Password);
            if (validUser == null)
                return Unauthorized();

            // Tạo token JWT
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                   new Claim(ClaimTypes.Name, validUser.UserName)
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes("YourSecretKey")), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            validUser.Token = tokenHandler.WriteToken(token);

            return Ok(validUser);
        }
    }
    public class AuthMiddleware
    {
        // Định nghĩa middleware cho xác thực token
    }
}
