using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IntranetPortal.Data.Data;
using IntranetPortal.Data.Models;

namespace IntranetPortal.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class SetupController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public SetupController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("quick-setup")]
        public async Task<IActionResult> QuickSetup([FromBody] QuickSetupDto dto)
        {
            if (dto == null) return BadRequest("Invalid bulk payload.");

            int sitesAdded = 0, deptsAdded = 0, rolesAdded = 0, posAdded = 0, permsAdded = 0;

            // 1. Process Sites
            if (dto.Sites != null)
            {
                foreach (var s in dto.Sites.Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => x.Trim()))
                {
                    if (!await _context.Sites.AnyAsync(x => x.Name.ToLower() == s.ToLower()))
                    {
                        _context.Sites.Add(new Site { Name = s });
                        sitesAdded++;
                    }
                }
            }

            // 2. Process Departments
            if (dto.Departments != null)
            {
                foreach (var d in dto.Departments.Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => x.Trim()))
                {
                    if (!await _context.Departments.AnyAsync(x => x.Name.ToLower() == d.ToLower()))
                    {
                        _context.Departments.Add(new Department { Name = d });
                        deptsAdded++;
                    }
                }
            }

            // 3. Process Roles
            if (dto.Roles != null)
            {
                foreach (var r in dto.Roles.Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => x.Trim()))
                {
                    if (!await _context.Roles.AnyAsync(x => x.Name.ToLower() == r.ToLower()))
                    {
                        _context.Roles.Add(new Role { Name = r });
                        rolesAdded++;
                    }
                }
            }

            // 4. Process Positions
            if (dto.Positions != null)
            {
                foreach (var p in dto.Positions.Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => x.Trim()))
                {
                    if (!await _context.Positions.AnyAsync(x => x.Name.ToLower() == p.ToLower()))
                    {
                        _context.Positions.Add(new Position { Name = p });
                        posAdded++;
                    }
                }
            }

            // 5. Process Permissions
            if (dto.Permissions != null)
            {
                foreach (var perm in dto.Permissions.Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => x.Trim()))
                {
                    if (!await _context.Permissions.AnyAsync(x => x.Name.ToLower() == perm.ToLower()))
                    {
                        _context.Permissions.Add(new Permission { Name = perm });
                        permsAdded++;
                    }
                }
            }

            if (sitesAdded + deptsAdded + rolesAdded + posAdded + permsAdded > 0)
            {
                await _context.SaveChangesAsync();
            }

            return Ok(new 
            { 
                Message = "Quick Setup transaction executed successfully.",
                Stats = new { sites = sitesAdded, departments = deptsAdded, roles = rolesAdded, positions = posAdded, permissions = permsAdded }
            });
        }
    }

    public class QuickSetupDto
    {
        public List<string> Roles { get; set; } = new();
        public List<string> Positions { get; set; } = new();
        public List<string> Departments { get; set; } = new();
        public List<string> Sites { get; set; } = new();
        public List<string> Permissions { get; set; } = new();
    }
}
