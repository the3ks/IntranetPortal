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
                .Select(s => new { s.Id, s.Name })
                .OrderBy(s => s.Name)
                .ToListAsync();

            return Ok(sites);
        }
    }
}
