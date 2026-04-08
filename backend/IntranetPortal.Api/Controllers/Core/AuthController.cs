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

            var activeDelegations = await _context.RoleDelegations
                .Include(rd => rd.UserRole)
                    .ThenInclude(ur => ur.Role)
                        .ThenInclude(r => r.RolePermissions)
                            .ThenInclude(rp => rp.Permission)
                .Where(rd => rd.SubstituteUserId == user.Id && 
                             rd.IsActive && 
                             rd.StartDate <= DateTimeOffset.UtcNow && 
                             rd.EndDate >= DateTimeOffset.UtcNow)
                .ToListAsync();

            var token = GenerateJwt(user, activeDelegations);
            
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

        private string GenerateJwt(IntranetPortal.Data.Models.UserAccount user, List<IntranetPortal.Data.Models.RoleDelegation>? delegations = null)
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

            var allUserRoles = user.UserRoles.ToList();
            if (delegations != null)
            {
                allUserRoles.AddRange(delegations.Select(d => d.UserRole).Where(ur => ur != null));
            }

            // Inject backwards compatibility Admin boolean for immediate frontend support
            if (allUserRoles.Any(ur => ur.Role?.Name == "Admin"))
            {
                claims.Add(new Claim(ClaimTypes.Role, "Admin"));
            }

            // Inject the precise granular Scoped Permissions explicitly mapping capabilities to strict organizational boundaries!
            var rolePerms = allUserRoles
                .Where(ur => ur.Role != null && ur.Role.RolePermissions != null)
                .SelectMany(ur => ur.Role.RolePermissions.Where(rp => rp.Permission != null).Select(rp => new 
                { 
                    PermName = rp.Permission.Name, 
                    ur.SiteId,
                    ur.DepartmentId
                })).ToList();

            // Distinct ScopedPerm (Functional) constraints
            var scopedClaims = rolePerms.Where(x => !x.DepartmentId.HasValue)
                .Select(x => $"{x.PermName}:" + (x.SiteId.HasValue ? x.SiteId.Value.ToString() : "Global")).Distinct();
            foreach (var sp in scopedClaims)
            {
                claims.Add(new Claim("ScopedPerm", sp));
            }

            // Distinct DeptPerm (Hierarchical) constraints
            var deptClaims = rolePerms.Where(x => x.DepartmentId.HasValue)
                .Select(x => $"{x.PermName}:{x.DepartmentId!.Value}").Distinct();
            foreach (var dp in deptClaims)
            {
                claims.Add(new Claim("DeptPerm", dp));
            }

            // Retain the generic un-scoped 'Permission' list strictly allowing [Authorize(Policy="...")] attributes to function natively
            foreach (var p in rolePerms.Select(x => x.PermName).Distinct())
            {
                claims.Add(new Claim("Permission", p));
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
