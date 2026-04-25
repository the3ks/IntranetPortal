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
    public class AttendanceController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public AttendanceController(ApplicationDbContext context) => _context = context;

        private async Task<Employee?> GetCurrentEmployeeRecord()
        {
            var subClaim = User.FindFirst(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sub)?.Value;
            if (string.IsNullOrEmpty(subClaim) || !int.TryParse(subClaim, out int userId)) return null;
            return await _context.Employees.FirstOrDefaultAsync(r => r.UserAccountId == userId);
        }

        [HttpGet("today")]
        public async Task<IActionResult> GetTodayStatus()
        {
            var employee = await GetCurrentEmployeeRecord();
            if (employee == null) return NotFound(new AttendanceTodayDto { HasRecord = false });

            var today = DateTime.UtcNow.Date;
            var log = await _context.HR_AttendanceLogs
                .FirstOrDefaultAsync(a => a.EmployeeId == employee.Id && a.Date == today);

            var pendingLeaveCount = await _context.HR_LeaveRequests
                .CountAsync(r => r.EmployeeId == employee.Id && r.Status == "Pending");

            return Ok(new AttendanceTodayDto
            {
                HasRecord = true,
                IsClockedIn = log?.ClockInTime != null && log?.ClockOutTime == null,
                ClockInTime = log?.ClockInTime,
                ClockOutTime = log?.ClockOutTime,
                PendingLeaveCount = pendingLeaveCount,
                Date = today
            });
        }

        [HttpPost("clock-in")]
        public async Task<IActionResult> ClockIn()
        {
            var employee = await GetCurrentEmployeeRecord();
            if (employee == null) return NotFound(new { Message = "No HR record found. Please contact HR." });

            var today = DateTime.UtcNow.Date;
            var existing = await _context.HR_AttendanceLogs
                .FirstOrDefaultAsync(a => a.EmployeeId == employee.Id && a.Date == today);

            if (existing?.ClockInTime != null)
                return BadRequest(new { Message = "You have already clocked in today." });

            if (existing == null)
            {
                existing = new AttendanceLog { EmployeeId = employee.Id, Date = today };
                _context.HR_AttendanceLogs.Add(existing);
            }

            existing.ClockInTime = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return Ok(new ClockInResultDto { ClockInTime = existing.ClockInTime!.Value, Message = "Clocked in successfully." });
        }

        [HttpPost("clock-out")]
        public async Task<IActionResult> ClockOut()
        {
            var employee = await GetCurrentEmployeeRecord();
            if (employee == null) return NotFound(new { Message = "No HR record found." });

            var today = DateTime.UtcNow.Date;
            var log = await _context.HR_AttendanceLogs
                .FirstOrDefaultAsync(a => a.EmployeeId == employee.Id && a.Date == today);

            if (log?.ClockInTime == null) return BadRequest(new { Message = "You have not clocked in yet." });
            if (log.ClockOutTime != null) return BadRequest(new { Message = "You have already clocked out today." });

            log.ClockOutTime = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            var duration = log.ClockOutTime.Value - log.ClockInTime!.Value;
            return Ok(new ClockOutResultDto
            {
                ClockOutTime = log.ClockOutTime.Value,
                Duration = $"{(int)duration.TotalHours}h {duration.Minutes}m",
                Message = "Clocked out successfully. Have a great rest of your day!"
            });
        }

        [HttpGet("history")]
        public async Task<IActionResult> GetHistory([FromQuery] int days = 14)
        {
            var employee = await GetCurrentEmployeeRecord();
            if (employee == null) return NotFound(new { Message = "No HR record found." });

            var since = DateTime.UtcNow.Date.AddDays(-days);
            var logs = await _context.HR_AttendanceLogs
                .Where(a => a.EmployeeId == employee.Id && a.Date >= since)
                .OrderByDescending(a => a.Date)
                .ToListAsync();

            return Ok(logs.Select(l => new AttendanceLogDto
            {
                Id = l.Id,
                Date = l.Date,
                ClockInTime = l.ClockInTime,
                ClockOutTime = l.ClockOutTime
            }));
        }
    }

    public class AttendanceTodayDto
    {
        public bool HasRecord { get; set; }
        public bool IsClockedIn { get; set; }
        public DateTime? ClockInTime { get; set; }
        public DateTime? ClockOutTime { get; set; }
        public int PendingLeaveCount { get; set; }
        public DateTime Date { get; set; }
    }

    public class AttendanceLogDto
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public DateTime? ClockInTime { get; set; }
        public DateTime? ClockOutTime { get; set; }
    }

    public class ClockInResultDto
    {
        public DateTime ClockInTime { get; set; }
        public string Message { get; set; } = string.Empty;
    }

    public class ClockOutResultDto
    {
        public DateTime ClockOutTime { get; set; }
        public string Duration { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
    }
}
