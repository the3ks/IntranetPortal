using IntranetPortal.Data.Data;
using IntranetPortal.Data.Models.HR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IntranetPortal.Api.Controllers.HR
{
    [ApiController]
    [Route("api/hr/[controller]")]
    [Authorize]
    public class OnboardingController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public OnboardingController(ApplicationDbContext context) => _context = context;

        private async Task<Employee?> GetCurrentEmployeeRecord()
        {
            var subClaim = User.FindFirst(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sub)?.Value;
            if (string.IsNullOrEmpty(subClaim) || !int.TryParse(subClaim, out int userId)) return null;

            return await _context.UserAccounts
                .Where(u => u.Id == userId)
                .Select(u => u.Employee)
                .FirstOrDefaultAsync();
        }

        private static OnboardingTemplateDto ToTemplateDto(OnboardingTemplate t) => new()
        {
            Id = t.Id,
            Name = t.Name,
            Description = t.Description,
            IsActive = t.IsActive,
            Tasks = t.Tasks.Select(task => new OnboardingTaskDto
            {
                Id = task.Id,
                TemplateId = task.TemplateId,
                Title = task.Title,
                Description = task.Description,
                AssigneeRole = task.AssigneeRole,
                Order = task.Order
            }).OrderBy(x => x.Order).ToList()
        };

        private static EmployeeOnboardingTaskDto ToEmployeeTaskDto(EmployeeOnboardingTask t) => new()
        {
            Id = t.Id,
            EmployeeId = t.EmployeeId,
            Title = t.OnboardingTask?.Title ?? string.Empty,
            Description = t.OnboardingTask?.Description,
            AssigneeRole = t.OnboardingTask?.AssigneeRole ?? "Employee",
            Order = t.OnboardingTask?.Order ?? 0,
            IsCompleted = t.IsCompleted,
            CompletedAt = t.CompletedAt
        };

        [HttpGet("templates")]
        public async Task<IActionResult> GetTemplates()
        {
            var templates = await _context.HR_OnboardingTemplates
                .Include(t => t.Tasks)
                .ToListAsync();
            return Ok(templates.Select(ToTemplateDto));
        }

        [HttpPost("templates")]
        public async Task<IActionResult> CreateTemplate([FromBody] OnboardingTemplateUpsertDto dto)
        {
            var template = new OnboardingTemplate { Name = dto.Name, Description = dto.Description, IsActive = dto.IsActive };
            _context.HR_OnboardingTemplates.Add(template);
            await _context.SaveChangesAsync();
            return Ok(ToTemplateDto(template));
        }

        [HttpPost("templates/{templateId}/tasks")]
        public async Task<IActionResult> AddTask(int templateId, [FromBody] OnboardingTaskUpsertDto dto)
        {
            var task = new OnboardingTask
            {
                TemplateId = templateId,
                Title = dto.Title,
                Description = dto.Description,
                AssigneeRole = dto.AssigneeRole,
                Order = dto.Order
            };
            _context.HR_OnboardingTasks.Add(task);
            await _context.SaveChangesAsync();
            return Ok(new OnboardingTaskDto { Id = task.Id, TemplateId = task.TemplateId, Title = task.Title, Description = task.Description, AssigneeRole = task.AssigneeRole, Order = task.Order });
        }

        [HttpPost("assign/{employeeRecordId}/{templateId}")]
        public async Task<IActionResult> AssignTemplate(int employeeRecordId, int templateId)
        {
            var tasks = await _context.HR_OnboardingTasks
                .Where(t => t.TemplateId == templateId)
                .ToListAsync();

            var assignments = tasks.Select(t => new EmployeeOnboardingTask
            {
                EmployeeId = employeeRecordId,
                OnboardingTaskId = t.Id,
                IsCompleted = false
            }).ToList();

            _context.HR_EmployeeOnboardingTasks.AddRange(assignments);
            await _context.SaveChangesAsync();
            return Ok(new { AssignedCount = assignments.Count });
        }

        [HttpGet("my-tasks")]
        public async Task<IActionResult> GetMyTasks()
        {
            var employee = await GetCurrentEmployeeRecord();
            if (employee == null) return NotFound(new { Message = "No HR Record." });

            var tasks = await _context.HR_EmployeeOnboardingTasks
                .Include(t => t.OnboardingTask)
                .Where(t => t.EmployeeId == employee.Id)
                .OrderBy(t => t.OnboardingTask!.Order)
                .ToListAsync();

            return Ok(tasks.Select(ToEmployeeTaskDto));
        }

        [HttpPost("tasks/{id}/complete")]
        public async Task<IActionResult> CompleteTask(int id)
        {
            var employee = await GetCurrentEmployeeRecord();
            if (employee == null) return Unauthorized();

            var task = await _context.HR_EmployeeOnboardingTasks
                .FirstOrDefaultAsync(t => t.Id == id && t.EmployeeId == employee.Id);

            if (task == null) return NotFound();

            task.IsCompleted = true;
            task.CompletedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return Ok(ToEmployeeTaskDto(task));
        }
    }

    public class OnboardingTaskDto
    {
        public int Id { get; set; }
        public int TemplateId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string AssigneeRole { get; set; } = "Employee";
        public int Order { get; set; }
    }

    public class OnboardingTemplateDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool IsActive { get; set; }
        public List<OnboardingTaskDto> Tasks { get; set; } = new();
    }

    public class OnboardingTemplateUpsertDto
    {
        public required string Name { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; } = true;
    }

    public class OnboardingTaskUpsertDto
    {
        public required string Title { get; set; }
        public string? Description { get; set; }
        public string AssigneeRole { get; set; } = "Employee";
        public int Order { get; set; }
    }

    public class EmployeeOnboardingTaskDto
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string AssigneeRole { get; set; } = "Employee";
        public int Order { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime? CompletedAt { get; set; }
    }
}
