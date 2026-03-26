using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IntranetPortal.Data.Data;
using IntranetPortal.Data.Models;

namespace IntranetPortal.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class SetupController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public SetupController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("quick-setup")]
        public async Task<IActionResult> QuickSetup([FromBody] QuickSetupDto dto)
        {
            if (dto == null) return BadRequest("Invalid bulk payload.");

            int sitesAdded = 0, deptsAdded = 0, rolesAdded = 0, posAdded = 0, permsAdded = 0;

            // 1. Process Sites
            if (dto.Sites != null)
            {
                foreach (var s in dto.Sites.Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => x.Trim()))
                {
                    if (!await _context.Sites.AnyAsync(x => x.Name.ToLower() == s.ToLower()))
                    {
                        _context.Sites.Add(new Site { Name = s });
                        sitesAdded++;
                    }
                }
            }

            // Force Entity Framework to structurally lock the new Sites into MySQL right now so we can extract their auto-incremented Identity keys physically.
            if (sitesAdded > 0)
            {
                await _context.SaveChangesAsync();
            }

            // Determine the explicit Localized Sequence explicitly for Departments.
            int? targetSiteId = null;
            string? firstDtoSite = dto.Sites?.FirstOrDefault(x => !string.IsNullOrWhiteSpace(x))?.Trim();
            
            if (!string.IsNullOrEmpty(firstDtoSite))
            {
                targetSiteId = await _context.Sites.Where(x => x.Name.ToLower() == firstDtoSite.ToLower()).Select(x => x.Id).FirstOrDefaultAsync();
            }
            
            if (targetSiteId == null || targetSiteId == 0)
            {
                // Fallback to the very first global site natively if the user didn't specify one in the bulk payload explicitly.
                targetSiteId = await _context.Sites.Select(x => x.Id).FirstOrDefaultAsync();
            }

            if (targetSiteId == null || targetSiteId == 0)
            {
                if (dto.Departments != null && dto.Departments.Where(x => !string.IsNullOrWhiteSpace(x)).Any())
                {
                    return BadRequest("A physical Site must be established before Departments can be mapped onto the corporate matrix.");
                }
            }

            // 2. Process Departments
            if (dto.Departments != null && targetSiteId.HasValue && targetSiteId.Value > 0)
            {
                foreach (var d in dto.Departments.Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => x.Trim()))
                {
                    if (!await _context.Departments.AnyAsync(x => x.Name.ToLower() == d.ToLower() && x.SiteId == targetSiteId.Value))
                    {
                        _context.Departments.Add(new Department { Name = d, SiteId = targetSiteId.Value });
                        deptsAdded++;
                    }
                }
            }

            // 3. Process Roles
            if (dto.Roles != null)
            {
                foreach (var r in dto.Roles.Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => x.Trim()))
                {
                    if (!await _context.Roles.AnyAsync(x => x.Name.ToLower() == r.ToLower()))
                    {
                        _context.Roles.Add(new Role { Name = r });
                        rolesAdded++;
                    }
                }
            }

            // 4. Process Positions
            if (dto.Positions != null)
            {
                foreach (var p in dto.Positions.Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => x.Trim()))
                {
                    if (!await _context.Positions.AnyAsync(x => x.Name.ToLower() == p.ToLower()))
                    {
                        _context.Positions.Add(new Position { Name = p });
                        posAdded++;
                    }
                }
            }

            // 5. Process Permissions
            if (dto.Permissions != null)
            {
                foreach (var perm in dto.Permissions.Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => x.Trim()))
                {
                    if (!await _context.Permissions.AnyAsync(x => x.Name.ToLower() == perm.ToLower()))
                    {
                        _context.Permissions.Add(new Permission { Name = perm });
                        permsAdded++;
                    }
                }
            }

            if (sitesAdded + deptsAdded + rolesAdded + posAdded + permsAdded > 0)
            {
                await _context.SaveChangesAsync();
            }

            return Ok(new 
            { 
                Message = "Quick Setup transaction executed successfully.",
                Stats = new { sites = sitesAdded, departments = deptsAdded, roles = rolesAdded, positions = posAdded, permissions = permsAdded }
            });
        }

        [HttpPost("import-employees")]
        public async Task<IActionResult> ImportEmployees(IFormFile file)
        {
            if (file == null || file.Length == 0) return BadRequest("Native CSV file is structurally empty or missing.");

            using var reader = new StreamReader(file.OpenReadStream());
            using var csv = new CsvHelper.CsvReader(reader, System.Globalization.CultureInfo.InvariantCulture);
            
            var records = csv.GetRecords<CsvEmployeeRecord>().ToList();
            var addedCount = 0;
            var updatedCount = 0;
            var importedList = new List<object>();

            foreach (var rec in records)
            {
                if (string.IsNullOrWhiteSpace(rec.Email) || string.IsNullOrWhiteSpace(rec.FullName)) continue;

                var siteName = string.IsNullOrWhiteSpace(rec.Site) ? "Head Office" : rec.Site.Trim();
                var site = await _context.Sites.FirstOrDefaultAsync(s => s.Name.ToLower() == siteName.ToLower());
                if (site == null)
                {
                    site = new Site { Name = siteName };
                    _context.Sites.Add(site);
                    await _context.SaveChangesAsync();
                }

                var deptName = string.IsNullOrWhiteSpace(rec.Department) ? "General" : rec.Department.Trim();
                var dept = await _context.Departments.FirstOrDefaultAsync(d => d.Name.ToLower() == deptName.ToLower() && d.SiteId == site.Id);
                if (dept == null)
                {
                    dept = new Department { Name = deptName, SiteId = site.Id };
                    _context.Departments.Add(dept);
                    await _context.SaveChangesAsync();
                }

                Team? team = null;
                if (!string.IsNullOrWhiteSpace(rec.Team))
                {
                    var teamName = rec.Team.Trim();
                    team = await _context.Teams.FirstOrDefaultAsync(t => t.Name.ToLower() == teamName.ToLower() && t.DepartmentId == dept.Id);
                    if (team == null)
                    {
                        team = new Team { Name = teamName, DepartmentId = dept.Id };
                        _context.Teams.Add(team);
                        await _context.SaveChangesAsync();
                    }
                }

                Position? position = null;
                if (!string.IsNullOrWhiteSpace(rec.Position))
                {
                    var posName = rec.Position.Trim();
                    position = await _context.Positions.FirstOrDefaultAsync(p => p.Name.ToLower() == posName.ToLower());
                    if (position == null)
                    {
                        position = new Position { Name = posName };
                        _context.Positions.Add(position);
                        await _context.SaveChangesAsync();
                    }
                }

                var isNew = false;
                var isModified = false;
                var employee = await _context.Employees.FirstOrDefaultAsync(e => e.Email.ToLower() == rec.Email.ToLower().Trim());
                
                if (employee == null)
                {
                    employee = new Employee
                    {
                        FullName = rec.FullName.Trim(),
                        Email = rec.Email.Trim(),
                        PositionId = position?.Id,
                        DepartmentId = dept.Id,
                        TeamId = team?.Id,
                        SiteId = site.Id
                    };
                    _context.Employees.Add(employee);
                    addedCount++;
                    isNew = true;

                    var allowStr = rec.AllowLogin?.Trim().ToLower() ?? "yes";
                    if (allowStr == "yes" || allowStr == "true" || allowStr == "1" || allowStr == "y")
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
                }
                else
                {
                    var allowStr = rec.AllowLogin?.Trim().ToLower() ?? "yes";
                    if (allowStr == "yes" || allowStr == "true" || allowStr == "1" || allowStr == "y")
                    {
                        var existingAcc = await _context.UserAccounts.FirstOrDefaultAsync(u => u.Email == employee.Email);
                        if (existingAcc == null)
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
                    }
                    if (employee.FullName != rec.FullName.Trim() ||
                        employee.PositionId != position?.Id ||
                        employee.DepartmentId != dept.Id ||
                        employee.TeamId != team?.Id ||
                        employee.SiteId != site.Id)
                    {
                        employee.FullName = rec.FullName.Trim();
                        employee.PositionId = position?.Id;
                        employee.DepartmentId = dept.Id;
                        employee.TeamId = team?.Id;
                        employee.SiteId = site.Id;
                        updatedCount++;
                        isModified = true;
                    }
                }

                importedList.Add(new {
                    FullName = employee.FullName,
                    Email = employee.Email,
                    PositionName = position?.Name ?? "Unassigned",
                    DepartmentName = dept.Name,
                    SiteName = site.Name,
                    Action = isNew ? "Inserted" : (isModified ? "Updated" : "Skipped")
                });
            }

            if (addedCount > 0 || updatedCount > 0)
            {
                await _context.SaveChangesAsync();
            }

            return Ok(new { 
                Message = "CSV Stream digested gracefully.", 
                Inserted = addedCount, 
                Updated = updatedCount,
                Skipped = records.Count - addedCount - updatedCount,
                Employees = importedList
            });
        }
    }

    public class QuickSetupDto
    {
        public List<string> Roles { get; set; } = new();
        public List<string> Positions { get; set; } = new();
        public List<string> Departments { get; set; } = new();
        public List<string> Sites { get; set; } = new();
        public List<string> Permissions { get; set; } = new();
    }

    public class CsvEmployeeRecord
    {
        [CsvHelper.Configuration.Attributes.Name("FullName")]
        public string FullName { get; set; } = string.Empty;
        
        [CsvHelper.Configuration.Attributes.Name("Email")]
        public string Email { get; set; } = string.Empty;
        
        [CsvHelper.Configuration.Attributes.Name("Position")]
        public string? Position { get; set; }
        
        [CsvHelper.Configuration.Attributes.Name("Department")]
        public string? Department { get; set; }
        
        [CsvHelper.Configuration.Attributes.Name("Team")]
        public string? Team { get; set; }
        
        [CsvHelper.Configuration.Attributes.Name("Site")]
        public string? Site { get; set; }

        [CsvHelper.Configuration.Attributes.Name("Allow Login")]
        public string? AllowLogin { get; set; }
    }
}
