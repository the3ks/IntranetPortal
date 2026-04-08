using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IntranetPortal.Data.Data;
using IntranetPortal.Api.Security;

namespace IntranetPortal.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class DepartmentsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IntranetPortal.Api.Security.IPermissionService _permissionService;

        public DepartmentsController(ApplicationDbContext context, IntranetPortal.Api.Security.IPermissionService permissionService)
        {
            _context = context;
            _permissionService = permissionService;
        }

        [HttpGet]
        public async Task<IActionResult> GetDepartments()
        {
            var departments = await _context.Departments
                .AsQueryable()
                .ApplySiteScope(_permissionService, "Structure.Department.View")
                .Select(d => new { d.Id, d.Name, d.SiteId })
                .OrderBy(d => d.Name)
                .ToListAsync();

            return Ok(departments);
        }

        [HttpPost]
        [Authorize(Policy = "Perm:Structure.Department.Create")]
        public async Task<IActionResult> CreateDepartment([FromBody] DepartmentDto dto)
        {
            if (!_permissionService.ValidateSiteScope("Structure.Department.Create", dto.SiteId)) return Forbid();

            var dept = new IntranetPortal.Data.Models.Department { Name = dto.Name, SiteId = dto.SiteId };
            _context.Departments.Add(dept);
            await _context.SaveChangesAsync();
            return Ok(new { dept.Id, dept.Name, dept.SiteId });
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "Perm:Structure.Department.Edit")]
        public async Task<IActionResult> UpdateDepartment(int id, [FromBody] DepartmentDto dto)
        {
            var dept = await _context.Departments.FindAsync(id);
            if (dept == null) return NotFound();

            if (!_permissionService.ValidateSiteScope("Structure.Department.Edit", dept.SiteId)) return Forbid();

            if (dto.SiteId != dept.SiteId) {
                if (!_permissionService.ValidateSiteScope("Structure.Department.Edit", dto.SiteId)) return Forbid();
            }

            dept.Name = dto.Name;
            dept.SiteId = dto.SiteId;
            await _context.SaveChangesAsync();
            return Ok(new { dept.Id, dept.Name, dept.SiteId });
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "Perm:Structure.Department.Delete")]
        public async Task<IActionResult> DeleteDepartment(int id)
        {
            var dept = await _context.Departments.FindAsync(id);
            if (dept == null) return NotFound();

            if (!_permissionService.ValidateSiteScope("Structure.Department.Delete", dept.SiteId)) return Forbid();

            _context.Departments.Remove(dept);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }

    public class DepartmentDto
    {
        public required string Name { get; set; }
        public int SiteId { get; set; }
    }
}
