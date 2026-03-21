using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IntranetPortal.Data.Data;

namespace IntranetPortal.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class SitesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public SitesController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetSites()
        {
            var sites = await _context.Sites
                .Select(s => new { s.Id, s.Name, s.Address })
                .OrderBy(s => s.Name)
                .ToListAsync();

            return Ok(sites);
        }

        [HttpPost]
        [Authorize(Policy = "Perm:Admin")]
        public async Task<IActionResult> CreateSite([FromBody] SiteDto dto)
        {
            var site = new IntranetPortal.Data.Models.Site { Name = dto.Name, Address = dto.Address };
            _context.Sites.Add(site);
            await _context.SaveChangesAsync();
            return Ok(new { site.Id, site.Name, site.Address });
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "Perm:Admin")]
        public async Task<IActionResult> UpdateSite(int id, [FromBody] SiteDto dto)
        {
            var site = await _context.Sites.FindAsync(id);
            if (site == null) return NotFound();

            site.Name = dto.Name;
            site.Address = dto.Address;
            await _context.SaveChangesAsync();
            return Ok(new { site.Id, site.Name, site.Address });
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "Perm:Admin")]
        public async Task<IActionResult> DeleteSite(int id)
        {
            var site = await _context.Sites.FindAsync(id);
            if (site == null) return NotFound();

            _context.Sites.Remove(site);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }

    public class SiteDto
    {
        public required string Name { get; set; }
        public string? Address { get; set; }
    }
}
