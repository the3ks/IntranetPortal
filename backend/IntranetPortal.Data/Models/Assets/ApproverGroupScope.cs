namespace IntranetPortal.Data.Models.Assets
{
    public class ApproverGroupScope
    {
        public int ApproverGroupId { get; set; }
        public ApproverGroup? ApproverGroup { get; set; }

        public int DepartmentId { get; set; }
        public Department? Department { get; set; }
    }
}
