namespace IntranetPortal.Data.Models
{
    public class AuditLog
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        public string? Email { get; set; }
        public string? IPAddress { get; set; }
        public required string Action { get; set; }
        public DateTimeOffset Timestamp { get; set; } = DateTimeOffset.UtcNow;
        public string? UserAgent { get; set; }
    }
}
