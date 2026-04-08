namespace IntranetPortal.Data.Models
{
    public class UserAccount
    {
        public int Id { get; set; }
        public required string Email { get; set; }
        public required string PasswordHash { get; set; }
        
        // Advanced Relational Role Matrix
        public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
        
        public bool IsActive { get; set; } = true;

        // Foreign Key to the Employee record
        public int? EmployeeId { get; set; }
        public Employee? Employee { get; set; }
    }
}
