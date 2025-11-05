using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        // Temporary in-memory users for demo
        private static readonly List<User> Users = new()
        {
            new User { Id = Guid.NewGuid(), Email = "admin@example.com", Password = "123456" },
            new User { Id = Guid.NewGuid(), Email = "user@example.com", Password = "password" }
        };

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            var user = Users.FirstOrDefault(u =>
                u.Email.Equals(request.Email, StringComparison.OrdinalIgnoreCase) &&
                u.Password == request.Password);

            if (user == null)
                return Unauthorized(new { message = "Invalid email or password" });

            // Normally you would return a JWT token here
            return Ok(new
            {
                message = "Login successful",
                user = new { user.Id, user.Email }
            });
        }
    }

    public class LoginRequest
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class User
    {
        public Guid Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
