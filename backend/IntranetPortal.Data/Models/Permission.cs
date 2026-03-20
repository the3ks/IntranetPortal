namespace IntranetPortal.Data.Models
{
    public class Permission
    {
        public int Id { get; set; }
        public required string Name { get; set; } // e.g. "HR.Approve", "Asset.Write"
        public string? Description { get; set; }

        public ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
    }
}
