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
    public class EmployeesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IPermissionService _permissionService;

        public EmployeesController(ApplicationDbContext context, IPermissionService permissionService)
        {
            _context = context;
            _permissionService = permissionService;
        }

        private static EmployeeDto ToDto(Employee e) => new()
        {
            Id = e.Id,
            FullName = e.FullName,
            Email = e.Email,
            EmployeeNumber = e.EmployeeNumber,
            HireDate = e.HireDate,
            DateOfBirth = e.DateOfBirth,
            EmergencyContactName = e.EmergencyContactName,
            EmergencyContactPhone = e.EmergencyContactPhone,
            UserAccountId = e.UserAccountId,
            DepartmentId = e.DepartmentId,
            DepartmentName = e.Department?.Name,
            PositionId = e.PositionId,
            PositionName = e.Position?.Name,
            TeamId = e.TeamId,
            TeamName = e.Team?.Name,
            SiteId = e.SiteId,
            SiteName = e.Site?.Name,
            DirectManagerId = e.DirectManagerId,
            DirectManagerName = e.DirectManager?.FullName
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
        public async Task<IActionResult> GetAll([FromQuery] string? search)
        {
            var userSiteId = await ResolveCurrentUserSiteId();

            var query = _context.Employees
                .Include(e => e.Department)
                .Include(e => e.Position)
                .Include(e => e.Team)
                .Include(e => e.Site)
                .Include(e => e.DirectManager)
                .AsQueryable();

            // Public directory behavior: permit own-site visibility even without explicit HR.Employee.View grants.
            query = query.ApplySiteScope(_permissionService, "HR.Employee.View", userSiteId);

            if (!string.IsNullOrWhiteSpace(search))
            {
                var lowerSearch = search.ToLower();
                query = query.Where(e => e.FullName.ToLower().Contains(lowerSearch) || 
                                         e.Email.ToLower().Contains(lowerSearch) ||
                                         e.EmployeeNumber.ToLower().Contains(lowerSearch));
            }

            var employees = await query.OrderByDescending(e => e.Id).ToListAsync();
            return Ok(employees.Select(ToDto));
        }

        [HttpGet("me")]
        public async Task<IActionResult> GetMyRecord()
        {
            var subClaim = User.FindFirst(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sub)?.Value;
            if (string.IsNullOrEmpty(subClaim) || !int.TryParse(subClaim, out int userId)) return Unauthorized();

            var record = await _context.Employees
                .Include(e => e.Department)
                .Include(e => e.Position)
                .Include(e => e.Team)
                .Include(e => e.Site)
                .Include(e => e.DirectManager)
                .FirstOrDefaultAsync(e => e.UserAccountId == userId);

            if (record == null) return NotFound(new { Message = "No HR record found for your account." });
            return Ok(ToDto(record));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var userSiteId = await ResolveCurrentUserSiteId();

            var record = await _context.Employees
                .Include(e => e.Department)
                .Include(e => e.Position)
                .Include(e => e.Team)
                .Include(e => e.Site)
                .Include(e => e.DirectManager)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (record == null) return NotFound();

            var canAccessByPermission = _permissionService.ValidateSiteScope("HR.Employee.View", record.SiteId);
            var canAccessByOwnSite = userSiteId.HasValue && record.SiteId == userSiteId.Value;
            if (!canAccessByPermission && !canAccessByOwnSite) return Forbid();

            return Ok(ToDto(record));
        }

        [HttpPost]
        [Authorize(Policy = "Perm:HR.Employee.Create")]
        public async Task<IActionResult> Create([FromBody] EmployeeUpsertDto dto)
        {
            if (!_permissionService.ValidateSiteScope("HR.Employee.Create", dto.SiteId)) return Forbid();

            // 1. Create or Find UserAccount
            var userAccount = await _context.UserAccounts.FirstOrDefaultAsync(u => u.Email == dto.Email.ToLower());
            if (userAccount == null)
            {
                userAccount = new IntranetPortal.Data.Models.UserAccount
                {
                    Email = dto.Email.ToLower(),
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Welcome2026!"),
                    IsActive = true
                };
                _context.UserAccounts.Add(userAccount);
                await _context.SaveChangesAsync();
            }

            // 2. Create Unified Employee Record
            var employee = new Employee
            {
                UserAccountId = userAccount.Id,
                FullName = dto.FullName,
                Email = dto.Email.ToLower(),
                EmployeeNumber = dto.EmployeeNumber,
                HireDate = dto.HireDate,
                DateOfBirth = dto.DateOfBirth,
                EmergencyContactName = dto.EmergencyContactName,
                EmergencyContactPhone = dto.EmergencyContactPhone,
                DepartmentId = dto.DepartmentId,
                PositionId = dto.PositionId,
                TeamId = dto.TeamId,
                SiteId = dto.SiteId,
                DirectManagerId = dto.DirectManagerId
            };

            _context.Employees.Add(employee);
            
            // Link back to user account if not already linked
            if (userAccount.EmployeeId == null) userAccount.EmployeeId = employee.Id;

            await _context.SaveChangesAsync();
            return Ok(ToDto(employee));
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "Perm:HR.Employee.Edit")]
        public async Task<IActionResult> Update(int id, [FromBody] EmployeeUpsertDto dto)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null) return NotFound();

            if (!_permissionService.ValidateSiteScope("HR.Employee.Edit", employee.SiteId)) return Forbid();
            if (dto.SiteId != employee.SiteId && !_permissionService.ValidateSiteScope("HR.Employee.Edit", dto.SiteId)) return Forbid();

            employee.FullName = dto.FullName;
            employee.Email = dto.Email.ToLower();
            employee.EmployeeNumber = dto.EmployeeNumber;
            employee.HireDate = dto.HireDate;
            employee.DateOfBirth = dto.DateOfBirth;
            employee.EmergencyContactName = dto.EmergencyContactName;
            employee.EmergencyContactPhone = dto.EmergencyContactPhone;
            employee.DepartmentId = dto.DepartmentId;
            employee.PositionId = dto.PositionId;
            employee.TeamId = dto.TeamId;
            employee.SiteId = dto.SiteId;
            employee.DirectManagerId = dto.DirectManagerId;

            await _context.SaveChangesAsync();
            return Ok(ToDto(employee));
        }
    }

    public class EmployeeDto
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string EmployeeNumber { get; set; } = string.Empty;
        public DateTime? HireDate { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? EmergencyContactName { get; set; }
        public string? EmergencyContactPhone { get; set; }
        public int UserAccountId { get; set; }
        public int? DepartmentId { get; set; }
        public string? DepartmentName { get; set; }
        public int? PositionId { get; set; }
        public string? PositionName { get; set; }
        public int? TeamId { get; set; }
        public string? TeamName { get; set; }
        public int? SiteId { get; set; }
        public string? SiteName { get; set; }
        public int? DirectManagerId { get; set; }
        public string? DirectManagerName { get; set; }
    }

    public class EmployeeUpsertDto
    {
        public required string FullName { get; set; }
        public required string Email { get; set; }
        public required string EmployeeNumber { get; set; }
        public int SiteId { get; set; }
        public int? DepartmentId { get; set; }
        public int? PositionId { get; set; }
        public int? TeamId { get; set; }
        public int? DirectManagerId { get; set; }
        public DateTime? HireDate { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? EmergencyContactName { get; set; }
        public string? EmergencyContactPhone { get; set; }
    }
}
