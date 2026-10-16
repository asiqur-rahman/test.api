using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Mail;
using System.Security.Claims;
using System.Text;
using Test.Core.Entities.Business.Accounts.Dtos;
using Test.Core.Entities.Common.User;
using Test.Core.Interfaces.Services;

namespace Test.Api.Controllers.Business
{
    [Route("[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IAccountService _accountService;

        public AccountController(IConfiguration configuration, IAccountService accountService)
        {
            _configuration = configuration;
            _accountService = accountService;
        }

        [AllowAnonymous]
        [HttpPost("Register", Name = "UserRegister")]
        public async Task<IActionResult> Register([FromBody] RegisterDto model)
        {
            if (model == null || string.IsNullOrWhiteSpace(model.Username) || string.IsNullOrWhiteSpace(model.Password))
                return BadRequest(new { message = "Username and password are required." });

            var username = model.Username.Trim();
            if (username.Length > 100)
                return BadRequest(new { message = "Username must be 100 characters or fewer." });
            if (model.Password.Length < 6)
                return BadRequest(new { message = "Password must be at least 6 characters." });

            var email = string.IsNullOrWhiteSpace(model.Email) ? null : model.Email.Trim();
            if (email != null && !IsValidEmail(email))
                return BadRequest(new { message = "Email is not valid." });

            var user = await _accountService.RegisterAsync(username, model.Password, email);
            if (user == null)
                return Conflict(new { message = "Username is already taken." });

            return Ok(new
            {
                id = user.Id,
                username = user.UserName,
                email = user.Email
            });
        }

        [AllowAnonymous]
        [HttpPost("Login", Name = "UserLogin")]
        public async Task<IActionResult> Login([FromBody] LoginDto model)
        {
            if (model == null || string.IsNullOrWhiteSpace(model.Username) || string.IsNullOrWhiteSpace(model.Password))
                return BadRequest(new { message = "Username and password are required." });

            var user = await _accountService.LoginAsync(model.Username, model.Password);
            if (user == null)
                return Unauthorized(new { message = "Invalid credentials." });

            var (token, expiration) = CreateToken(user);
            return Ok(new
            {
                token,
                expiration,
                username = user.UserName
            });
        }

        [Authorize]
        [HttpGet("me")]
        public IActionResult Me()
        {
            var username = User?.Identity?.Name ?? "unknown";
            return Ok(new { username });
        }

        private (string Token, DateTime Expiration) CreateToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtKey = _configuration["Jwt:Key"] ?? throw new InvalidOperationException("Jwt:Key is required in configuration");
            var key = Encoding.UTF8.GetBytes(jwtKey);
            var expires = DateTime.UtcNow.AddHours(8);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.Role, "User")
                }),
                Expires = expires,
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return (tokenHandler.WriteToken(token), expires);
        }

        private static bool IsValidEmail(string email)
        {
            try
            {
                _ = new MailAddress(email);
                return email.Contains('@');
            }
            catch
            {
                return false;
            }
        }
    }
}
