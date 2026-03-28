using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IntranetPortal.Data.Data;
using IntranetPortal.Data.Models;

namespace IntranetPortal.Api.Controllers
{
    [Authorize(Policy = "Perm:Admin.System.Access")]
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UsersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/users?search=name&filter=elevated|basic
        [HttpGet]
        public async Task<IActionResult> GetUsers([FromQuery] string? search, [FromQuery] string filter = "elevated")
        {
            var query = _context.UserAccounts
                .Include(u => u.Employee)
                .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Site)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                var lowerSearch = search.ToLower();
                query = query.Where(u => 
                    u.Email.ToLower().Contains(lowerSearch) || 
                    (u.Employee != null && u.Employee.FullName.ToLower().Contains(lowerSearch))
                );
            }

            if (filter.ToLower() == "basic")
            {
                // Basic Staff have exactly ZERO security roles explicitly defined
                query = query.Where(u => u.UserRoles.Count() == 0);
            }
            else // Default to elevated 
            {
                // Elevated Rights naturally hold at least one defined explicit operational role mechanically
                query = query.Where(u => u.UserRoles.Count() > 0);
            }

            var users = await query
                .OrderByDescending(u => u.Id)
                .Select(u => new 
                {
                    u.Id,
                    u.Email,
                    u.IsActive,
                    EmployeeId = u.EmployeeId,
                    EmployeeName = u.Employee != null ? u.Employee.FullName : "System Account",
                    Roles = u.UserRoles.Select(ur => new 
                    {
                        ur.Id,
                        ur.RoleId,
                        RoleName = ur.Role.Name,
                        SiteId = ur.SiteId,
                        SiteName = ur.Site != null ? ur.Site.Name : "Global Scope"
                    })
                })
                .ToListAsync();

            return Ok(users);
        }

        // POST: api/users/{id}/roles
        [HttpPost("{id}/roles")]
        public async Task<IActionResult> AssignRole(int id, [FromBody] RoleAssignmentDto dto)
        {
            var user = await _context.UserAccounts.Include(u => u.UserRoles).FirstOrDefaultAsync(u => u.Id == id);
            if (user == null) return NotFound("Software Identity Account inherently missing.");

            // Verify requested parameters exist safely
            var roleExists = await _context.Roles.AnyAsync(r => r.Id == dto.RoleId);
            if (!roleExists) return BadRequest("The requested analytical capability matrix does not structurally exist.");

            if (dto.SiteId.HasValue)
            {
                var siteExists = await _context.Sites.AnyAsync(s => s.Id == dto.SiteId);
                if (!siteExists) return BadRequest("The specified physical Geographic Boundary does not structurally exist.");
            }

            // Check if this precise Role/Site mapping already exists dynamically
            if (user.UserRoles.Any(ur => ur.RoleId == dto.RoleId && ur.SiteId == dto.SiteId))
            {
                return BadRequest("This exact granular capability matrix is already assigned identically to this account.");
            }

            user.UserRoles.Add(new UserRole
            {
                RoleId = dto.RoleId,
                SiteId = dto.SiteId
            });

            await _context.SaveChangesAsync();
            return Ok(new { Message = "Operational constraint formally bound to Identity Matrix seamlessly." });
        }

        // DELETE: api/users/{accountId}/roles/{roleMappingId}
        [HttpDelete("{accountId}/roles/{roleMappingId}")]
        public async Task<IActionResult> RemoveRole(int accountId, int roleMappingId)
        {
            var mapping = await _context.UserRoles.FirstOrDefaultAsync(ur => ur.Id == roleMappingId && ur.UserAccountId == accountId);
            if (mapping == null) return NotFound();

            _context.UserRoles.Remove(mapping);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // POST: api/users/{id}/reset-password
        [HttpPost("{id}/reset-password")]
        public async Task<IActionResult> ResetPassword(int id, [FromBody] PasswordResetDto dto)
        {
            var user = await _context.UserAccounts.FindAsync(id);
            if (user == null) return NotFound("Software Identity Account inherently missing.");

            if (string.IsNullOrWhiteSpace(dto.NewPassword) || dto.NewPassword.Length < 6)
                return BadRequest("Secure boundaries require standard multi-character metrics minimum perfectly exactly.");

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);
            await _context.SaveChangesAsync();
            return Ok(new { Message = "Authentication credentials structurally redefined flawlessly." });
        }
    }

    public class RoleAssignmentDto
    {
        public int RoleId { get; set; }
        public int? SiteId { get; set; }
    }

    public class PasswordResetDto
    {
        public string NewPassword { get; set; } = string.Empty;
    }
}
