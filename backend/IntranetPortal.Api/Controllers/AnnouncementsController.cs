using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IntranetPortal.Data.Data;
using IntranetPortal.Data.Models;
using System.Security.Claims;
using IntranetPortal.Api.Security;

namespace IntranetPortal.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class AnnouncementsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IntranetPortal.Api.Security.IPermissionService _permissionService;

        public AnnouncementsController(ApplicationDbContext context, IntranetPortal.Api.Security.IPermissionService permissionService)
        {
            _context = context;
            _permissionService = permissionService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAnnouncements([FromQuery] int limit = 50)
        {
            var query = _context.Announcements.Include(a => a.Author).AsQueryable();

            int? userSiteId = null;
            var empClaim = User.Claims.FirstOrDefault(c => c.Type == "employeeId")?.Value;
            if (!string.IsNullOrEmpty(empClaim) && int.TryParse(empClaim, out int empId))
            {
                userSiteId = await _context.Employees.Where(e => e.Id == empId).Select(e => (int?)e.SiteId).FirstOrDefaultAsync();
            }

            query = query.ApplySiteScope(_permissionService, "Announcements.View", userSiteId);

            var announcements = await query
                .OrderByDescending(a => a.CreatedAt)
                .Take(limit)
                .Select(a => new {
                    a.Id,
                    a.Title,
                    a.Content,
                    a.CreatedAt,
                    a.SiteId,
                    AuthorId = a.AuthorId,
                    AuthorName = a.Author != null ? a.Author.FullName : "Corporate Communications"
                })
                .ToListAsync();

            return Ok(announcements);
        }

        [HttpPost]
        [Authorize(Policy = "Perm:Announcements.Create")]
        public async Task<IActionResult> CreateAnnouncement([FromBody] AnnouncementDto dto)
        {
            if (!_permissionService.ValidateSiteScope("Announcements.Create", dto.SiteId)) return Forbid();

            // Try extracting EmployeeId from token natively if possible (Claim "employeeId")
            var empClaim = User.Claims.FirstOrDefault(c => c.Type == "employeeId")?.Value;
            int authorId = dto.AuthorId;
            if (int.TryParse(empClaim, out int tokenEmpId)) {
                authorId = tokenEmpId;
            }

            var announcement = new Announcement 
            { 
                Title = dto.Title, 
                Content = dto.Content,
                CreatedAt = DateTime.UtcNow,
                SiteId = dto.SiteId,
                AuthorId = authorId > 0 ? authorId : 1 // Fallback to System/Admin ID 1
            };
            
            _context.Announcements.Add(announcement);
            await _context.SaveChangesAsync();
            return Ok(new { announcement.Id, announcement.Title, announcement.SiteId });
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "Perm:Announcements.Delete")]
        public async Task<IActionResult> DeleteAnnouncement(int id)
        {
            var announcement = await _context.Announcements.FindAsync(id);
            if (announcement == null) return NotFound();

            if (!_permissionService.ValidateSiteScope("Announcements.Delete", announcement.SiteId)) return Forbid();

            _context.Announcements.Remove(announcement);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }

    public class AnnouncementDto
    {
        public required string Title { get; set; }
        public required string Content { get; set; }
        public int AuthorId { get; set; } = 1;
        public int? SiteId { get; set; }
    }
}
