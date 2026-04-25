namespace IntranetPortal.Data.Models.HR
{
    public class LeaveRequest
    {
        public int Id { get; set; }
        
        public int EmployeeId { get; set; }
        public Employee? Employee { get; set; }

        public int LeaveTypeId { get; set; }
        public LeaveType? LeaveType { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        
        public required string Reason { get; set; }

        // Pending, Approved, Rejected
        public string Status { get; set; } = "Pending";

        public int? ApprovedById { get; set; }
        public Employee? ApprovedBy { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
