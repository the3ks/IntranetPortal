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
    public class RoleDelegationsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public RoleDelegationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/roledelegations/user/{userId}
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetUserDelegations(int userId)
        {
            var delegations = await _context.RoleDelegations
                .Include(rd => rd.SubstituteUser)
                    .ThenInclude(u => u.Employee)
                .Include(rd => rd.UserRole)
                    .ThenInclude(ur => ur.Role)
                .Include(rd => rd.UserRole)
                    .ThenInclude(ur => ur.Site)
                .Include(rd => rd.UserRole)
                    .ThenInclude(ur => ur.Department)
                .Where(rd => rd.SourceUserId == userId)
                .OrderByDescending(rd => rd.StartDate)
                .Select(rd => new
                {
                    rd.Id,
                    SubstituteUserId = rd.SubstituteUserId,
                    SubstituteName = rd.SubstituteUser.Employee != null ? rd.SubstituteUser.Employee.FullName : rd.SubstituteUser.Email,
                    UserRoleId = rd.UserRoleId,
                    RoleName = rd.UserRole.Role.Name,
                    SiteName = rd.UserRole.Site != null ? rd.UserRole.Site.Name : "Global Scope",
                    DepartmentName = rd.UserRole.Department != null ? rd.UserRole.Department.Name : null,
                    rd.StartDate,
                    rd.EndDate,
                    rd.IsActive,
                    IsExpired = rd.EndDate < DateTimeOffset.UtcNow
                })
                .ToListAsync();

            return Ok(delegations);
        }

        // POST: api/roledelegations
        [HttpPost]
        public async Task<IActionResult> CreateDelegation([FromBody] CreateDelegationDto dto)
        {
            if (dto.SourceUserId == dto.SubstituteUserId)
                return BadRequest("Source User cannot delegate to themselves.");
            if (dto.StartDate >= dto.EndDate)
                return BadRequest("End Date must be strictly after the Start Date timeline.");
            
            var userRole = await _context.UserRoles.FirstOrDefaultAsync(ur => ur.Id == dto.UserRoleId && ur.UserAccountId == dto.SourceUserId);
            if (userRole == null)
                return BadRequest("The explicitly requested User Role cannot be delegated because it is not actively mapped to the Source User profile.");

            var substituteExists = await _context.UserAccounts.AnyAsync(u => u.Id == dto.SubstituteUserId);
            if (!substituteExists)
                return BadRequest("Target Substitute account structurally missing.");

            // Check for overlapping delegations of the exact same role to the exact same substitute
            var overlapping = await _context.RoleDelegations.AnyAsync(rd => 
                rd.SourceUserId == dto.SourceUserId &&
                rd.SubstituteUserId == dto.SubstituteUserId &&
                rd.UserRoleId == dto.UserRoleId &&
                rd.IsActive &&
                rd.StartDate < dto.EndDate && 
                rd.EndDate > dto.StartDate
            );

            if (overlapping)
                return BadRequest("An overlapping active temporal delegation already exists bridging this precise matrix array.");

            var delegation = new RoleDelegation
            {
                SourceUserId = dto.SourceUserId,
                SubstituteUserId = dto.SubstituteUserId,
                UserRoleId = dto.UserRoleId,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                IsActive = true
            };

            _context.RoleDelegations.Add(delegation);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Temporal Capability substitution injected correctly into proxy.", Id = delegation.Id });
        }

        // DELETE: api/roledelegations/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> RevokeDelegation(int id)
        {
            var delegation = await _context.RoleDelegations.FindAsync(id);
            if (delegation == null) return NotFound("Matrix linkage structurally missing.");

            _context.RoleDelegations.Remove(delegation);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }

    public class CreateDelegationDto
    {
        public int SourceUserId { get; set; }
        public int SubstituteUserId { get; set; }
        public int UserRoleId { get; set; }
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset EndDate { get; set; }
    }
}
