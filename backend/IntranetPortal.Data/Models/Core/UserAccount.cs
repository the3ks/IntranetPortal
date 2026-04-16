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

        // Security: incremented on password reset or account disable to invalidate existing tokens
        public int SecurityStamp { get; set; } = 1;

        // Security: brute-force lockout tracking
        public int FailedLoginAttempts { get; set; } = 0;
        public DateTimeOffset? LockedUntil { get; set; }

        // Foreign Key to the Employee record
        public int? EmployeeId { get; set; }
        public Employee? Employee { get; set; }
    }
}
