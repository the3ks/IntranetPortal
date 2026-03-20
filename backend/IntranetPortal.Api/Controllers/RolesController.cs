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
    public class RolesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public RolesController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetRoles()
        {
            var roles = await _context.Roles
                .Select(r => new { r.Id, r.Name, r.Description })
                .OrderBy(r => r.Name)
                .ToListAsync();

            return Ok(roles);
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole([FromBody] RoleDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var role = new Role { Name = dto.Name, Description = dto.Description };
            _context.Roles.Add(role);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetRoles), new { id = role.Id }, role);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRole(int id)
        {
            var role = await _context.Roles.FindAsync(id);
            if (role == null) return NotFound();

            // Permanent core application integrity lock
            if (role.Name == "Admin" || role.Name == "Staff") 
            {
                return BadRequest(new { Message = "Core system architecture roles cannot be forcefully dismantled." });
            }

            _context.Roles.Remove(role);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }

    public class RoleDto
    {
        public required string Name { get; set; }
        public string? Description { get; set; }
    }
}
