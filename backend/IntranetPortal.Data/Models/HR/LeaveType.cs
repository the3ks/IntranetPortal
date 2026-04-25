namespace IntranetPortal.Data.Models.HR
{
    public class LeaveType
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        
        // Annual allowance in days
        public int DaysAllowed { get; set; }

        public bool RequiresApproval { get; set; } = true;
    }
}
