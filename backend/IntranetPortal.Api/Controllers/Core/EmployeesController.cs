using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IntranetPortal.Data.Data;
using IntranetPortal.Data.Models;
using IntranetPortal.Api.Security;

namespace IntranetPortal.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IntranetPortal.Api.Security.IPermissionService _permissionService;

        public EmployeesController(ApplicationDbContext context, IntranetPortal.Api.Security.IPermissionService permissionService)
        {
            _context = context;
            _permissionService = permissionService;
        }

        // GET: api/employees
        [HttpGet]
        [Authorize(Policy = "Perm:HR.Employee.View")]
        public async Task<IActionResult> GetEmployees([FromQuery] string? search)
        {
            var query = _context.Employees
                .AsQueryable()
                .ApplySiteScope(_permissionService, "HR.Employee.View");

            if (!string.IsNullOrWhiteSpace(search))
            {
                var lowerSearch = search.ToLower();
                query = query.Where(e => e.FullName.ToLower().Contains(lowerSearch) || e.Email.ToLower().Contains(lowerSearch));
            }

            var employees = await query
                .Include(e => e.Position)
                .Include(e => e.Department)
                .Include(e => e.Team)
                .Include(e => e.Site)
                .Select(e => new 
                {
                    e.Id,
                    e.FullName,
                    e.Email,
                    PositionId = e.PositionId,
                    PositionName = e.Position != null ? e.Position.Name : "Unassigned",
                    DepartmentId = e.DepartmentId,
                    DepartmentName = e.Department != null ? e.Department.Name : "Unassigned",
                    TeamId = e.TeamId,
                    TeamName = e.Team != null ? e.Team.Name : "Unassigned",
                    SiteId = e.SiteId,
                    SiteName = e.Site != null ? e.Site.Name : "Unassigned"
                })
                .OrderByDescending(e => e.Id)
                .ToListAsync();

            return Ok(employees);
        }

        // GET: api/employees/{id}
        [HttpGet("{id}")]
        [Authorize(Policy = "Perm:HR.Employee.View")]
        public async Task<IActionResult> GetEmployee(int id)
        {
            var query = _context.Employees
                .AsQueryable()
                .ApplySiteScope(_permissionService, "HR.Employee.View");

            var employee = await query
                .Where(e => e.Id == id)
                .Select(e => new 
                { 
                    e.Id, 
                    e.FullName, 
                    e.Email, 
                    e.PositionId, 
                    e.DepartmentId, 
                    e.TeamId, 
                    e.SiteId,
                    allowLogin = _context.UserAccounts.Any(u => u.EmployeeId == e.Id && u.IsActive)
                })
                .FirstOrDefaultAsync();

            if (employee == null) return NotFound();
            return Ok(employee);
        }

        // POST: api/employees
        [HttpPost]
        [Authorize(Policy = "Perm:HR.Employee.Create")]
        public async Task<IActionResult> CreateEmployee([FromBody] EmployeeDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (!_permissionService.ValidateSiteScope("HR.Employee.Create", dto.SiteId)) return Forbid();

            var employee = new Employee
            {
                FullName = dto.FullName,
                Email = dto.Email,
                PositionId = dto.PositionId,
                DepartmentId = dto.DepartmentId,
                TeamId = dto.TeamId,
                SiteId = dto.SiteId
            };

            _context.Employees.Add(employee);

            if (dto.AllowLogin)
            {
                var userAcc = new UserAccount
                {
                    Email = employee.Email,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Welcome2026!"),
                    IsActive = true,
                    Employee = employee
                };
                _context.UserAccounts.Add(userAcc);
            }

            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetEmployees), new { id = employee.Id }, employee);
        }

        // PUT: api/employees/{id}
        [HttpPut("{id}")]
        [Authorize(Policy = "Perm:HR.Employee.Edit")]
        public async Task<IActionResult> UpdateEmployee(int id, [FromBody] EmployeeDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var employee = await _context.Employees.FindAsync(id);
            if (employee == null) return NotFound();

            // Verify permission on existing Site
            if (!_permissionService.ValidateSiteScope("HR.Employee.Edit", employee.SiteId)) return Forbid();
            
            // Verify permission on NEW target Site (if moving employee across branches)
            if (dto.SiteId != employee.SiteId && !_permissionService.ValidateSiteScope("HR.Employee.Edit", dto.SiteId)) return Forbid();

            employee.FullName = dto.FullName;
            employee.Email = dto.Email;
            employee.PositionId = dto.PositionId;
            employee.DepartmentId = dto.DepartmentId;
            employee.TeamId = dto.TeamId;
            employee.SiteId = dto.SiteId;

            var userAcc = await _context.UserAccounts.FirstOrDefaultAsync(u => u.EmployeeId == id);
            if (dto.AllowLogin)
            {
                if (userAcc == null)
                {
                    _context.UserAccounts.Add(new UserAccount
                    {
                        Email = employee.Email,
                        PasswordHash = BCrypt.Net.BCrypt.HashPassword("Welcome2026!"),
                        IsActive = true,
                        EmployeeId = employee.Id
                    });
                }
                else
                {
                    userAcc.IsActive = true;
                    userAcc.Email = employee.Email; // Sync email if changed structurally
                }
            }
            else
            {
                if (userAcc != null)
                {
                    userAcc.IsActive = false;
                }
            }

            await _context.SaveChangesAsync();
            return Ok(employee);
        }

        // DELETE: api/employees/{id}
        [HttpDelete("{id}")]
        [Authorize(Policy = "Perm:HR.Employee.Edit")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null) return NotFound();

            if (!_permissionService.ValidateSiteScope("HR.Employee.Edit", employee.SiteId)) return Forbid();

            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }

    public class EmployeeDto
    {
        public required string FullName { get; set; }
        public required string Email { get; set; }
        public int? PositionId { get; set; }
        public int DepartmentId { get; set; }
        public int? TeamId { get; set; }
        public int SiteId { get; set; }
        public bool AllowLogin { get; set; } = true;
    }
}
