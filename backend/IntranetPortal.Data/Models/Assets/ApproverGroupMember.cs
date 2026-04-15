namespace IntranetPortal.Data.Models.Assets
{
    public class ApproverGroupMember
    {
        public int ApproverGroupId { get; set; }
        public ApproverGroup? ApproverGroup { get; set; }

        public int EmployeeId { get; set; }
        public Employee? Employee { get; set; }
    }
}
