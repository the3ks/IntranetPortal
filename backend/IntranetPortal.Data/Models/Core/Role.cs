namespace IntranetPortal.Data.Models
{
    public class Role
    {
        public int Id { get; set; }
        public required string Name { get; set; } // e.g. "HR Manager", "Staff"
        public string? Description { get; set; }

        public ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
        public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    }
}
