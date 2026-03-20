using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Linq;
using IntranetPortal.Data.Data;
using IntranetPortal.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace IntranetPortal.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _config;
        private readonly IWebHostEnvironment _env;

        public AuthController(ApplicationDbContext context, IConfiguration config, IWebHostEnvironment env)
        {
            _context = context;
            _config = config;
            _env = env;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var user = await _context.UserAccounts
                .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                        .ThenInclude(r => r.RolePermissions)
                            .ThenInclude(rp => rp.Permission)
                .FirstOrDefaultAsync(u => u.Email == request.Email);
            
            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            {
                return Unauthorized(new { Message = "Invalid email or password." });
            }

            var token = GenerateJwt(user);
            
            // Temporary backward compatibility for the Next.js UI Role string
            var legacyRole = user.UserRoles.Any(ur => ur.Role.Name == "Admin") ? "Admin" : "Staff";
            return Ok(new { Token = token, Role = legacyRole });
        }

        [HttpPost("seed-test-admin")]
        public async Task<IActionResult> SeedTestAdmin()
        {
            // CRITICAL SECURITY PRECAUTION: Completely lock down reseeding if the Server is running in Production
            if (!_env.IsDevelopment())
            {
                return NotFound(new { Message = "Testing and Seeding endpoints are completely disabled outside of the Local Development environment." });
            }

            // Scrub exactly every legacy duplicate that could have accidentally spun up!
            var existingUsers = await _context.UserAccounts.Where(u => u.Email == "admin@company.com").ToListAsync();
            if (existingUsers.Any())
            {
                _context.UserAccounts.RemoveRange(existingUsers);
                await _context.SaveChangesAsync(); 
            }

            var adminRole = await _context.Roles.Include(r => r.RolePermissions).FirstOrDefaultAsync(r => r.Name == "Admin");
            if (adminRole == null) 
            {
                adminRole = new IntranetPortal.Data.Models.Role { Name = "Admin", Description = "Global Application Administrator" };
                
                // Seed a foundational Permission object to populate the Advanced RBAC Matrix
                var superAdminPerm = new IntranetPortal.Data.Models.Permission { Name = "System.FullAccess", Description = "God-mode capability" };
                _context.Permissions.Add(superAdminPerm);

                // Map the Permission tightly to the Role using the Many-to-Many entity wrapper
                adminRole.RolePermissions.Add(new IntranetPortal.Data.Models.RolePermission { Role = adminRole, Permission = superAdminPerm });
                
                _context.Roles.Add(adminRole);
            }

            var admin = new IntranetPortal.Data.Models.UserAccount
            {
                Email = "admin@company.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin123!"),
                IsActive = true,
                UserRoles = new List<IntranetPortal.Data.Models.UserRole>
                {
                    new IntranetPortal.Data.Models.UserRole { Role = adminRole, SiteId = null } // SiteId null = Global Scope!
                }
            };

            _context.UserAccounts.Add(admin);
            await _context.SaveChangesAsync();
            return Ok(new { Message = "Test admin user logically created equipped with Global Resource Scope!" });
        }

        private string GenerateJwt(IntranetPortal.Data.Models.UserAccount user)
        {
            var jwtSettings = _config.GetSection("JwtSettings");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("EmployeeId", user.EmployeeId?.ToString() ?? "")
            };

            // Inject backwards compatibility Admin boolean for immediate frontend support
            if (user.UserRoles.Any(ur => ur.Role?.Name == "Admin"))
            {
                claims.Add(new Claim(ClaimTypes.Role, "Admin"));
            }

            // Inject the precise granular Developer Constants array explicitly mapping capabilities efficiently!
            var permissions = user.UserRoles
                .Where(ur => ur.Role != null && ur.Role.RolePermissions != null)
                .SelectMany(ur => ur.Role.RolePermissions)
                .Where(rp => rp.Permission != null)
                .Select(rp => rp.Permission.Name)
                .Distinct();

            foreach (var perm in permissions)
            {
                claims.Add(new Claim("Permission", perm));
            }

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(8),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }

    public class LoginRequest
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
    }
}
