using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IntranetPortal.Data.Models.HR
{
    public class Employee : ISiteScoped, IDepartmentScoped
    {
        public int Id { get; set; }
        
        // 1-to-1 Mapping to Core UserAccount (Identity)
        public int UserAccountId { get; set; }
        public UserAccount? UserAccount { get; set; }

        // Core Personnel Data
        public required string FullName { get; set; }
        public required string Email { get; set; }
        
        // HR Specific Data
        public required string EmployeeNumber { get; set; }
        public DateTime? HireDate { get; set; }
        public DateTime? DateOfBirth { get; set; }

        public string? EmergencyContactName { get; set; }
        public string? EmergencyContactPhone { get; set; }

        // Organizational Mapping (Unified)
        public int? DepartmentId { get; set; }
        public Department? Department { get; set; }

        public int? PositionId { get; set; }
        public Position? Position { get; set; }

        public int? TeamId { get; set; }
        public Team? Team { get; set; }

        [Required]
        public int? SiteId { get; set; }
        public Site? Site { get; set; }

        // Reporting Line
        public int? DirectManagerId { get; set; }
        public Employee? DirectManager { get; set; }
        public ICollection<Employee> DirectReports { get; set; } = new List<Employee>();

        // Activity Relations
        public ICollection<Department> ManagedDepartments { get; set; } = new List<Department>();
        public ICollection<LeaveRequest> LeaveRequests { get; set; } = new List<LeaveRequest>();
        public ICollection<EmployeeOnboardingTask> OnboardingTasks { get; set; } = new List<EmployeeOnboardingTask>();
        public ICollection<AttendanceLog> AttendanceLogs { get; set; } = new List<AttendanceLog>();
    }
}
