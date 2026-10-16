using Microsoft.AspNetCore.Mvc;
using RaceDay.API.Data;
using RaceDay.API.Models;

namespace RaceDay.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AuthController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto request)
        {
            // 1. Check if user already exists
            if (_context.Users.Any(u => u.Email == request.Email))
            {
                return BadRequest("User with this email already exists.");
            }

            // 2. Hash the password (NEVER store plain text passwords)
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

            // 3. Create the new user
            var user = new User
            {
                Email = request.Email,
                PasswordHash = passwordHash,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Role = request.Role
            };

            // 4. Save to database
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "User registered successfully!" });
        }
        [HttpGet("organiser-only")]
        [RaceDay.API.Filters.SessionAuthorize("Organiser")]
        public IActionResult OrganiserOnlyEndpoint()
        {
            return Ok(new { Message = "Welcome, Organiser! You have accessed a restricted endpoint." });
        }
        [HttpPost("login")]
        public IActionResult Login(LoginDto request)
        {
            // 1. Find the user by email
            var user = _context.Users.FirstOrDefault(u => u.Email == request.Email);

            if (user == null)
            {
                return BadRequest("User not found.");
            }

            // 2. Verify the password against the stored hash
            if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            {
                return BadRequest("Wrong password.");
            }

            // 3. Set up the Session (Rubric Requirement!)
            HttpContext.Session.SetInt32("UserId", user.Id);
            HttpContext.Session.SetString("UserRole", user.Role);

            return Ok(new { Message = "Login successful!", Role = user.Role });
        }
    }
}