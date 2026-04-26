using IntranetPortal.Data.Data;
using IntranetPortal.Data.Models.HR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IntranetPortal.Api.Controllers.HR
{
    [ApiController]
    [Route("api/hr/leave")]
    [Authorize]
    public class LeaveManagementController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public LeaveManagementController(ApplicationDbContext context) => _context = context;

        private async Task<Employee?> GetCurrentEmployeeRecord()
        {
            var subClaim = User.FindFirst(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sub)?.Value;
            if (string.IsNullOrEmpty(subClaim) || !int.TryParse(subClaim, out int userId)) return null;

            return await _context.UserAccounts
                .Where(u => u.Id == userId)
                .Select(u => u.Employee)
                .FirstOrDefaultAsync();
        }

        private static LeaveTypeDto ToLeaveTypeDto(LeaveType t) => new()
        {
            Id = t.Id,
            Name = t.Name,
            Description = t.Description,
            DaysAllowed = t.DaysAllowed,
            RequiresApproval = t.RequiresApproval
        };

        private static LeaveRequestDto ToLeaveRequestDto(LeaveRequest r) => new()
        {
            Id = r.Id,
            EmployeeId = r.EmployeeId,
            EmployeeName = r.Employee?.FullName,
            LeaveTypeId = r.LeaveTypeId,
            LeaveTypeName = r.LeaveType?.Name,
            StartDate = r.StartDate,
            EndDate = r.EndDate,
            Reason = r.Reason,
            Status = r.Status,
            ApprovedById = r.ApprovedById,
            ApprovedByName = r.ApprovedBy?.FullName,
            CreatedAt = r.CreatedAt
        };

        [HttpGet("types")]
        public async Task<IActionResult> GetLeaveTypes()
        {
            var types = await _context.HR_LeaveTypes.ToListAsync();
            return Ok(types.Select(ToLeaveTypeDto));
        }

        [HttpPost("types")]
        public async Task<IActionResult> CreateLeaveType([FromBody] LeaveTypeUpsertDto dto)
        {
            var type = new LeaveType { Name = dto.Name, Description = dto.Description, DaysAllowed = dto.DaysAllowed, RequiresApproval = dto.RequiresApproval };
            _context.HR_LeaveTypes.Add(type);
            await _context.SaveChangesAsync();
            return Ok(ToLeaveTypeDto(type));
        }

        [HttpGet("my-requests")]
        public async Task<IActionResult> GetMyRequests()
        {
            var employee = await GetCurrentEmployeeRecord();
            if (employee == null) return NotFound(new { Message = "No HR Record." });

            var requests = await _context.HR_LeaveRequests
                .Include(r => r.LeaveType)
                .Include(r => r.Employee)
                .Include(r => r.ApprovedBy)
                .Where(r => r.EmployeeId == employee.Id)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();

            return Ok(requests.Select(ToLeaveRequestDto));
        }

        [HttpPost("request")]
        public async Task<IActionResult> SubmitRequest([FromBody] SubmitLeaveRequestDto dto)
        {
            var employee = await GetCurrentEmployeeRecord();
            if (employee == null) return NotFound(new { Message = "No HR Record." });

            var req = new LeaveRequest
            {
                EmployeeId = employee.Id,
                LeaveTypeId = dto.LeaveTypeId,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                Reason = dto.Reason,
                Status = "Pending",
                CreatedAt = DateTime.UtcNow
            };

            _context.HR_LeaveRequests.Add(req);
            await _context.SaveChangesAsync();
            return Ok(ToLeaveRequestDto(req));
        }

        [HttpGet("approvals")]
        public async Task<IActionResult> GetApprovals()
        {
            var employee = await GetCurrentEmployeeRecord();
            if (employee == null) return NotFound(new { Message = "No HR Record." });

            var requests = await _context.HR_LeaveRequests
                .Include(r => r.LeaveType)
                .Include(r => r.Employee).ThenInclude(e => e!.Department)
                .Include(r => r.ApprovedBy)
                .Where(r => r.Employee!.DirectManagerId == employee.Id ||
                            (r.Employee.Department != null && r.Employee.Department.ManagerId == employee.Id))
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();

            return Ok(requests.Select(ToLeaveRequestDto));
        }

        [HttpPost("{id}/approve")]
        public async Task<IActionResult> ApproveRequest(int id)
        {
            var req = await _context.HR_LeaveRequests.FindAsync(id);
            if (req == null) return NotFound();

            var employee = await GetCurrentEmployeeRecord();
            req.Status = "Approved";
            req.ApprovedById = employee?.Id;
            await _context.SaveChangesAsync();
            return Ok(new { req.Status });
        }

        [HttpPost("{id}/reject")]
        public async Task<IActionResult> RejectRequest(int id)
        {
            var req = await _context.HR_LeaveRequests.FindAsync(id);
            if (req == null) return NotFound();

            var employee = await GetCurrentEmployeeRecord();
            req.Status = "Rejected";
            req.ApprovedById = employee?.Id;
            await _context.SaveChangesAsync();
            return Ok(new { req.Status });
        }
    }

    public class LeaveTypeDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int DaysAllowed { get; set; }
        public bool RequiresApproval { get; set; }
    }

    public class LeaveTypeUpsertDto
    {
        public required string Name { get; set; }
        public string? Description { get; set; }
        public int DaysAllowed { get; set; }
        public bool RequiresApproval { get; set; } = true;
    }

    public class LeaveRequestDto
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public string? EmployeeName { get; set; }
        public int LeaveTypeId { get; set; }
        public string? LeaveTypeName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Reason { get; set; } = string.Empty;
        public string Status { get; set; } = "Pending";
        public int? ApprovedById { get; set; }
        public string? ApprovedByName { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class SubmitLeaveRequestDto
    {
        public int LeaveTypeId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public required string Reason { get; set; }
    }
}
