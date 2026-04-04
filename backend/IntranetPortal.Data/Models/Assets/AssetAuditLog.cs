using System;
using System.ComponentModel.DataAnnotations;

namespace IntranetPortal.Data.Models.Assets
{
    public class AssetAuditLog
    {
        public int Id { get; set; }

        public int AssetId { get; set; }
        public Asset? Asset { get; set; }

        [Required]
        [MaxLength(100)]
        public required string Action { get; set; }

        public string? OldValue { get; set; }
        public string? NewValue { get; set; }

        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        public int PerformedByEmployeeId { get; set; }
        public Employee? PerformedByEmployee { get; set; }
    }
}
