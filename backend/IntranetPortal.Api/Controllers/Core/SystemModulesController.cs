using IntranetPortal.Data.Data;
using IntranetPortal.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IntranetPortal.Api.Controllers.Core
{
    [ApiController]

    public class SystemModulesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public SystemModulesController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("api/modules")]
        [Authorize]
        public async Task<IActionResult> GetMyModules()
        {
            var siteIdStr = User.FindFirst("SiteId")?.Value;
            int? userSiteId = string.IsNullOrEmpty(siteIdStr) ? null : int.Parse(siteIdStr);

            IQueryable<SystemModule> query = _context.SystemModules
                .Where(m => m.IsActive);
            
            if (userSiteId.HasValue)
            {
                // Must be strictly allowed via Site Authorization matrix
                query = query.Where(m => m.AllowedSites.Any(s => s.Id == userSiteId.Value));
            }

            var modules = await query.OrderBy(m => m.Order).ToListAsync();
            return Ok(modules);
        }

        // --- Admin Endpoints ---

        [HttpGet("api/admin/modules")]
        [Authorize(Policy = "Perm:System.Modules.Manage")]
        public async Task<IActionResult> GetAllModulesForAdmin()
        {
            var modules = await _context.SystemModules
                .Include(m => m.AllowedSites)
                .OrderBy(m => m.Order)
                .ToListAsync();
            return Ok(modules);
        }

        [HttpPost("api/admin/modules")]
        [Authorize(Policy = "Perm:System.Modules.Manage")]
        public async Task<IActionResult> CreateModule([FromBody] SystemModule module)
        {
            _context.SystemModules.Add(module);
            await _context.SaveChangesAsync();
            return Ok(module);
        }

        [HttpPut("api/admin/modules/{id}")]
        [Authorize(Policy = "Perm:System.Modules.Manage")]
        public async Task<IActionResult> UpdateModule(int id, [FromBody] SystemModule updatedModule)
        {
            var module = await _context.SystemModules.Include(m => m.AllowedSites).FirstOrDefaultAsync(m => m.Id == id);
            if (module == null) return NotFound();

            module.Name = updatedModule.Name;
            module.Description = updatedModule.Description;
            module.Url = updatedModule.Url;
            module.IconSvg = updatedModule.IconSvg;
            module.IsActiveGlobally = updatedModule.IsActiveGlobally;
            module.IsActive = updatedModule.IsActive;
            module.Order = updatedModule.Order;

            // Simple many-to-many update (Assumes frontend sends bare allowed sites)
            module.AllowedSites.Clear();
            if (updatedModule.AllowedSites != null) {
                var siteIds = updatedModule.AllowedSites.Select(s => s.Id).ToList();
                var sites = await _context.Sites.Where(s => siteIds.Contains(s.Id)).ToListAsync();
                foreach(var s in sites) {
                    module.AllowedSites.Add(s);
                }
            }

            await _context.SaveChangesAsync();
            return Ok(module);
        }

        [HttpDelete("api/admin/modules/{id}")]
        [Authorize(Policy = "Perm:System.Modules.Manage")]
        public async Task<IActionResult> DeleteModule(int id)
        {
            var module = await _context.SystemModules.FindAsync(id);
            if (module == null) return NotFound();

            _context.SystemModules.Remove(module);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
