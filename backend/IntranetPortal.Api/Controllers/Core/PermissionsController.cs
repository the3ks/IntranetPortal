using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IntranetPortal.Data.Data;

namespace IntranetPortal.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class PermissionsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PermissionsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetPermissions()
        {
            var permissions = await _context.Permissions
                .OrderBy(p => p.Name)
                .Select(p => new { p.Id, p.Name, p.Description, p.IsObsolete })
                .ToListAsync();

            return Ok(permissions);
        }
    }
}
