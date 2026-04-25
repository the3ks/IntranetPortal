using IntranetPortal.Data.Data;
using IntranetPortal.Data.Models.HR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IntranetPortal.Api.Security;

namespace IntranetPortal.Api.Controllers.HR
{
    [ApiController]
    [Route("api/hr/[controller]")]
    [Authorize]
    public class DepartmentsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IPermissionService _permissionService;

        public DepartmentsController(ApplicationDbContext context, IPermissionService permissionService)
        {
            _context = context;
            _permissionService = permissionService;
        }

        private static DepartmentDto ToDto(Department d) => new()
        {
            Id = d.Id,
            Name = d.Name,
            Description = d.Description,
            ParentDepartmentId = d.ParentDepartmentId,
            ParentDepartmentName = d.ParentDepartment?.Name,
            ManagerId = d.ManagerId,
            ManagerName = d.Manager?.FullName,
            SiteId = d.SiteId,
            SiteName = d.Site?.Name
        };

        private async Task<int?> ResolveCurrentUserSiteId()
        {
            var empClaim = User.Claims.FirstOrDefault(c => c.Type == "EmployeeId")?.Value;
            if (!string.IsNullOrEmpty(empClaim) && int.TryParse(empClaim, out int empId))
            {
                return await _context.Employees
                    .Where(e => e.Id == empId)
                    .Select(e => (int?)e.SiteId)
                    .FirstOrDefaultAsync();
            }

            return null;
        }

        [HttpGet]
        public async Task<IActionResult> GetDepartments()
        {
            var userSiteId = await ResolveCurrentUserSiteId();

            var query = _context.Departments
                .Include(d => d.ParentDepartment)
                .Include(d => d.Manager)
                .Include(d => d.Site)
                .AsQueryable();

            // Public directory behavior: permit own-site visibility even without explicit HR.Employee.View grants.
            query = query.ApplySiteScope(_permissionService, "HR.Employee.View", userSiteId);

            var departments = await query.ToListAsync();
            return Ok(departments.Select(ToDto));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDepartment(int id)
        {
            var userSiteId = await ResolveCurrentUserSiteId();

            var dept = await _context.Departments
                .Include(d => d.ParentDepartment)
                .Include(d => d.Manager)
                .Include(d => d.Site)
                .Include(d => d.ChildDepartments)
                .Include(d => d.Employees)
                .FirstOrDefaultAsync(d => d.Id == id);

            if (dept == null) return NotFound();

            var canAccessByPermission = _permissionService.ValidateSiteScope("HR.Employee.View", dept.SiteId);
            var canAccessByOwnSite = userSiteId.HasValue && dept.SiteId == userSiteId.Value;
            if (!canAccessByPermission && !canAccessByOwnSite) return Forbid();

            return Ok(ToDto(dept));
        }

        [HttpPost]
        [Authorize(Policy = "Perm:System.ManageRoles")] // Using a higher level perm for org changes
        public async Task<IActionResult> CreateDepartment([FromBody] DepartmentUpsertDto dto)
        {
            if (!_permissionService.ValidateSiteScope("System.ManageRoles", dto.SiteId)) return Forbid();

            var dept = new Department 
            { 
                Name = dto.Name, 
                Description = dto.Description, 
                ParentDepartmentId = dto.ParentDepartmentId, 
                ManagerId = dto.ManagerId,
                SiteId = dto.SiteId
            };
            _context.Departments.Add(dept);
            await _context.SaveChangesAsync();
            return Ok(ToDto(dept));
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "Perm:System.ManageRoles")]
        public async Task<IActionResult> UpdateDepartment(int id, [FromBody] DepartmentUpsertDto dto)
        {
            var dept = await _context.Departments.FindAsync(id);
            if (dept == null) return NotFound();

            if (!_permissionService.ValidateSiteScope("System.ManageRoles", dept.SiteId)) return Forbid();
            if (dto.SiteId != dept.SiteId && !_permissionService.ValidateSiteScope("System.ManageRoles", dto.SiteId)) return Forbid();

            dept.Name = dto.Name;
            dept.Description = dto.Description;
            dept.ParentDepartmentId = dto.ParentDepartmentId;
            dept.ManagerId = dto.ManagerId;
            dept.SiteId = dto.SiteId;

            await _context.SaveChangesAsync();
            return Ok(ToDto(dept));
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "Perm:System.ManageRoles")]
        public async Task<IActionResult> DeleteDepartment(int id)
        {
            var dept = await _context.Departments.FindAsync(id);
            if (dept == null) return NotFound();

            if (!_permissionService.ValidateSiteScope("System.ManageRoles", dept.SiteId)) return Forbid();

            // Prevent deletion if employees are assigned
            var hasEmployees = await _context.Employees.AnyAsync(e => e.DepartmentId == id);
            if (hasEmployees) return BadRequest(new { Message = "Cannot delete a department with active employees." });

            _context.Departments.Remove(dept);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }

    public class DepartmentDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int? ParentDepartmentId { get; set; }
        public string? ParentDepartmentName { get; set; }
        public int? ManagerId { get; set; }
        public string? ManagerName { get; set; }
        public int? SiteId { get; set; }
        public string? SiteName { get; set; }
    }

    public class DepartmentUpsertDto
    {
        public required string Name { get; set; }
        public string? Description { get; set; }
        public int? ParentDepartmentId { get; set; }
        public int? ManagerId { get; set; }
        public int SiteId { get; set; }
    }
}
