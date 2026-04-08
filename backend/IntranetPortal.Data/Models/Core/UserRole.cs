namespace IntranetPortal.Data.Models
{
    public class UserRole
    {
        public int Id { get; set; }

        public int UserAccountId { get; set; }
        public UserAccount UserAccount { get; set; } = null!;

        public int RoleId { get; set; }
        public Role Role { get; set; } = null!;

        // The critical ABAC scoping parameter.
        // If NULL, the User assumes the Role globally across the portal.
        // If assigned a Value, the User assumes the Role explicitly for that local Branch/Site.
        public int? SiteId { get; set; }
        public Site? Site { get; set; }

        // Added for Hierarchical constraints
        public int? DepartmentId { get; set; }
        public Department? Department { get; set; }
    }
}
