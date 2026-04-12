using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IntranetPortal.Data.Data;
using IntranetPortal.Data.Models;

namespace IntranetPortal.Api.Controllers.Core
{
    [ApiController]
    [Route("api/internal/permissions")]
    public class InternalPermissionsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public InternalPermissionsController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterPermissions([FromBody] List<InternalPermissionDto> incomingPermissions)
        {
            var expectedSecret = _configuration["InternalApiSettings:Secret"];
            
            // Allow bypassing if not configured, or strictly enforce it
            if (string.IsNullOrEmpty(expectedSecret))
            {
                return StatusCode(500, new { message = "Server misconfiguration: InternalApiSettings:Secret is missing." });
            }

            if (!Request.Headers.TryGetValue("X-Internal-Secret", out var providedSecret) || 
                providedSecret != expectedSecret)
            {
                return Unauthorized(new { message = "Invalid or missing internal secret." });
            }

            if (incomingPermissions == null || !incomingPermissions.Any())
            {
                return BadRequest("Payload empty.");
            }

            var incomingNames = incomingPermissions.Select(p => p.Name).ToList();

            var existingPermissions = await _context.Permissions
                .Where(p => incomingNames.Contains(p.Name))
                .Select(p => p.Name)
                .ToListAsync();

            var newPermissions = incomingPermissions
                .Where(p => !existingPermissions.Contains(p.Name))
                .Select(p => new Permission
                {
                    Name = p.Name,
                    Description = p.Description
                })
                .ToList();

            if (newPermissions.Any())
            {
                _context.Permissions.AddRange(newPermissions);
                await _context.SaveChangesAsync();
            }

            return Ok(new { registered = newPermissions.Count, message = "Permissions synced successfully." });
        }
    }

    public class InternalPermissionDto
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}
