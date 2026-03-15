using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SafeVault.Data;
using SafeVault.DTOs;
using SafeVault.Security;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SafeVault.Controllers
{
    [ApiController]
    [Route("api")]
    public class AuthAndSubmitController : ControllerBase
    {
        private readonly Database _db;
        private readonly Sanitizer _sanitizer;
        private readonly Validator _validator;
        private readonly PasswordHasher _hasher;
        private readonly IConfiguration _config;

        public AuthAndSubmitController(Database db, Sanitizer sanitizer, Validator validator, PasswordHasher hasher, IConfiguration config)
        {
            _db = db;
            _sanitizer = sanitizer;
            _validator = validator;
            _hasher = hasher;
            _config = config;
        }

        [HttpPost("submit")]
        public IActionResult Submit([FromBody] Dto dto)
        {
            try
            {
                var username = _sanitizer.SanitizeInput(dto.Username);
                var email = _sanitizer.SanitizeInput(dto.Email);

                var userResult = _validator.ValidateUsername(username);
                if (!userResult.IsValid) return BadRequest(userResult.ErrorMessage);

                var emailResult = _validator.ValidateEmail(email);
                if (!emailResult.IsValid) return BadRequest(emailResult.ErrorMessage);

                _db.InsertUser(username, email);
                return Ok(new { message = "Saved safely" });
            }
            catch
            {
                return StatusCode(500, new { error = "An unexpected error occurred." });
            }
        }

        [HttpPost("auth/register")]
        public IActionResult Register([FromBody] RegisterDto dto)
        {
            try
            {
                var username = _sanitizer.SanitizeInput(dto.Username);
                var email = _sanitizer.SanitizeInput(dto.Email);
                var password = dto.Password ?? string.Empty;

                var userResult = _validator.ValidateUsername(username);
                if (!userResult.IsValid) return BadRequest(userResult.ErrorMessage);

                var emailResult = _validator.ValidateEmail(email);
                if (!emailResult.IsValid) return BadRequest(emailResult.ErrorMessage);

                var passwordResult = _validator.ValidatePassword(password);
                if (!passwordResult.IsValid) return BadRequest(passwordResult.ErrorMessage);

                // Activity 3 hardening:
                // Public registration must NEVER let the client choose admin.
                var role = "user";

                var hash = _hasher.HashPassword(password);

                _db.CreateUser(username, email, hash, role);

                return Ok(new { message = "User registered." });
            }
            catch
            {
                return StatusCode(500, new { error = "An unexpected error occurred." });
            }
        }

        [HttpPost("auth/login")]
        public IActionResult Login([FromBody] LoginDto dto)
        {
            try
            {
                var username = _sanitizer.SanitizeInput(dto.Username);
                var password = dto.Password ?? string.Empty;

                var userResult = _validator.ValidateUsername(username);
                if (!userResult.IsValid) return Unauthorized("Invalid username or password.");

                var user = _db.GetUserForLogin(username);
                if (user is null) return Unauthorized("Invalid username or password.");

                if (!_hasher.VerifyPassword(password, user.PasswordHash))
                    return Unauthorized("Invalid username or password.");

                var token = GenerateJwt(user.Username, user.Role);
                return Ok(new { token });
            }
            catch
            {
                return StatusCode(500, new { error = "An unexpected error occurred." });
            }
        }

        private string GenerateJwt(string username, string role)
        {
            var key = _config["Jwt:Key"]!;
            var issuer = _config["Jwt:Issuer"]!;
            var audience = _config["Jwt:Audience"]!;

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Role, role)
            };

            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}