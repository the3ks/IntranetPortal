using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Linq;
using IntranetPortal.Data.Data;
using IntranetPortal.Data.Models;
using Microsoft.EntityFrameworkCore;
using IntranetPortal.Api.Security;

namespace IntranetPortal.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _config;
        private readonly IWebHostEnvironment _env;
        private readonly IChallengeCryptoService _challengeCryptoService;

        public AuthController(ApplicationDbContext context, IConfiguration config, IWebHostEnvironment env, IChallengeCryptoService challengeCryptoService)
        {
            _context = context;
            _config = config;
            _env = env;
            _challengeCryptoService = challengeCryptoService;
        }

        [HttpPost("login")]
        [EnableRateLimiting("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            request.Email = NormalizeEmail(request.Email);

            var user = await LoadUserForAuthenticationAsync(request.Email);

            if (user == null)
            {
                return Unauthorized(new { Message = "Invalid email or password." });
            }

            if (user.LockedUntil.HasValue && user.LockedUntil > DateTimeOffset.UtcNow)
            {
                return LockedOutResponse();
            }

            if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            {
                RegisterFailedLogin(user);
                await _context.SaveChangesAsync();
                return Unauthorized(new { Message = "Invalid email or password." });
            }

            RegisterSuccessfulLogin(user);
            await _context.SaveChangesAsync();

            return await CreateLoginSuccessResponseAsync(user);
        }

        [HttpPost("challenge/start")]
        [EnableRateLimiting("login")]
        public async Task<IActionResult> StartChallenge([FromBody] ChallengeStartRequest request)
        {
            var normalizedEmail = NormalizeEmail(request.Email);
            var userId = await _context.UserAccounts
                .AsNoTracking()
                .Where(u => u.Email == normalizedEmail && u.IsActive)
                .Select(u => (int?)u.Id)
                .FirstOrDefaultAsync();

            var challenge = new LoginChallenge
            {
                ChallengeId = Guid.NewGuid().ToString("N"),
                NormalizedEmail = normalizedEmail,
                Nonce = Convert.ToBase64String(RandomNumberGenerator.GetBytes(24)),
                ExpiresAt = DateTimeOffset.UtcNow.AddSeconds(60),
                UserAccountId = userId
            };

            _context.LoginChallenges.Add(challenge);
            await _context.SaveChangesAsync();

            return Ok(new ChallengeStartResponse
            {
                ChallengeId = challenge.ChallengeId,
                Nonce = challenge.Nonce,
                ExpiresAt = challenge.ExpiresAt,
                Algorithm = _challengeCryptoService.Algorithm,
                PublicKeyPem = _challengeCryptoService.ExportPublicKeyPem()
            });
        }

        [HttpPost("challenge/complete")]
        [EnableRateLimiting("login")]
        public async Task<IActionResult> CompleteChallenge([FromBody] ChallengeCompleteRequest request)
        {
            request.Email = NormalizeEmail(request.Email);

            var challenge = await _context.LoginChallenges
                .FirstOrDefaultAsync(c => c.ChallengeId == request.ChallengeId && c.NormalizedEmail == request.Email);

            if (challenge == null || challenge.ConsumedAt.HasValue || challenge.ExpiresAt <= DateTimeOffset.UtcNow)
            {
                return Unauthorized(new { Message = "Invalid or expired login challenge." });
            }

            challenge.ConsumedAt = DateTimeOffset.UtcNow;

            var user = challenge.UserAccountId.HasValue
                ? await LoadUserForAuthenticationAsync(challenge.UserAccountId.Value)
                : null;

            if (user == null)
            {
                await _context.SaveChangesAsync();
                return Unauthorized(new { Message = "Invalid email or password." });
            }

            if (user.LockedUntil.HasValue && user.LockedUntil > DateTimeOffset.UtcNow)
            {
                await _context.SaveChangesAsync();
                return LockedOutResponse();
            }

            string password;
            try
            {
                password = _challengeCryptoService.DecryptPassword(request.EncryptedPassword);
            }
            catch (CryptographicException)
            {
                RegisterFailedLogin(user);
                await _context.SaveChangesAsync();
                return Unauthorized(new { Message = "Invalid email or password." });
            }
            catch (FormatException)
            {
                RegisterFailedLogin(user);
                await _context.SaveChangesAsync();
                return Unauthorized(new { Message = "Invalid email or password." });
            }

            if (!BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            {
                RegisterFailedLogin(user);
                await _context.SaveChangesAsync();
                return Unauthorized(new { Message = "Invalid email or password." });
            }

            RegisterSuccessfulLogin(user);
            await _context.SaveChangesAsync();

            return await CreateLoginSuccessResponseAsync(user);
        }

        private static string NormalizeEmail(string email)
        {
            return email.Trim().ToLower();
        }

        private async Task<UserAccount?> LoadUserForAuthenticationAsync(string normalizedEmail)
        {
            return await _context.UserAccounts
                .Include(u => u.Employee)
                .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                        .ThenInclude(r => r.RolePermissions)
                            .ThenInclude(rp => rp.Permission)
                .FirstOrDefaultAsync(u => u.Email == normalizedEmail && u.IsActive);
        }

        private async Task<UserAccount?> LoadUserForAuthenticationAsync(int userId)
        {
            return await _context.UserAccounts
                .Include(u => u.Employee)
                .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                        .ThenInclude(r => r.RolePermissions)
                            .ThenInclude(rp => rp.Permission)
                .FirstOrDefaultAsync(u => u.Id == userId && u.IsActive);
        }

        private static IActionResult LockedOutResponse()
        {
            return new UnauthorizedObjectResult(new { Message = "Account is temporarily locked. Please try again later." });
        }

        private static void RegisterFailedLogin(UserAccount user)
        {
            user.FailedLoginAttempts++;
            if (user.FailedLoginAttempts >= 10)
            {
                user.LockedUntil = DateTimeOffset.UtcNow.AddMinutes(15);
            }
        }

        private static void RegisterSuccessfulLogin(UserAccount user)
        {
            user.FailedLoginAttempts = 0;
            user.LockedUntil = null;
        }

        private async Task<IActionResult> CreateLoginSuccessResponseAsync(UserAccount user)
        {
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

            // Create a dummy environment framework to mount the Employee
            var testSite = await _context.Sites.FirstOrDefaultAsync(s => s.Name == "Global HQ");
            if (testSite == null)
            {
                testSite = new IntranetPortal.Data.Models.Site { Name = "Global HQ", Address = "HQ" };
                _context.Sites.Add(testSite);
                await _context.SaveChangesAsync();
            }

            var testDept = await _context.Departments.FirstOrDefaultAsync(d => d.Name == "IT Department");
            if (testDept == null)
            {
                testDept = new IntranetPortal.Data.Models.Department { Name = "IT Department", Description = "IT Department", Site = testSite };
                _context.Departments.Add(testDept);
                await _context.SaveChangesAsync();
            }

            var adminEmployee = new IntranetPortal.Data.Models.Employee
            {
                FullName = "System Administrator",
                Email = "admin@company.com",
                Site = testSite,
                Department = testDept
            };
            _context.Employees.Add(adminEmployee);

            var admin = new IntranetPortal.Data.Models.UserAccount
            {
                Email = "admin@company.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin123!"),
                IsActive = true,
                Employee = adminEmployee,
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
                new Claim("SecurityStamp", user.SecurityStamp.ToString()),
                new Claim("EmployeeId", user.EmployeeId?.ToString() ?? ""),
                new Claim("FullName", user.Employee?.FullName ?? ""),
                new Claim("SiteId", user.Employee?.SiteId.ToString() ?? ""),
                new Claim("DepartmentId", user.Employee?.DepartmentId.ToString() ?? ""),
                new Claim("TeamId", user.Employee?.TeamId?.ToString() ?? "")
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

    public class ChallengeStartRequest
    {
        public required string Email { get; set; }
    }

    public class ChallengeStartResponse
    {
        public required string ChallengeId { get; set; }
        public required string Nonce { get; set; }
        public DateTimeOffset ExpiresAt { get; set; }
        public required string Algorithm { get; set; }
        public required string PublicKeyPem { get; set; }
    }

    public class ChallengeCompleteRequest
    {
        public required string Email { get; set; }
        public required string ChallengeId { get; set; }
        public required string EncryptedPassword { get; set; }
    }
}
