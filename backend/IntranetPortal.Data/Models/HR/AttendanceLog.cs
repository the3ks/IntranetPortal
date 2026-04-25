namespace IntranetPortal.Data.Models.HR
{
    public class AttendanceLog
    {
        public int Id { get; set; }

        public int EmployeeId { get; set; }
        public Employee? Employee { get; set; }

        public DateTime Date { get; set; }

        public DateTime? ClockInTime { get; set; }
        public DateTime? ClockOutTime { get; set; }

        // Optional metadata like IP address or location could be added here
    }
}
