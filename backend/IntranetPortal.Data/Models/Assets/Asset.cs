using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IntranetPortal.Data.Models.Assets
{
    public class Asset : ISiteScoped, IDepartmentScoped
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public required string AssetTag { get; set; }

        public int ModelId { get; set; }
        public AssetModel? Model { get; set; }

        [MaxLength(100)]
        public string? SerialNumber { get; set; }

        public AssetStatus Status { get; set; } = AssetStatus.Available;

        [MaxLength(200)]
        public string? PhysicalLocation { get; set; }

        // RBAC Boundaries
        public int? SiteId { get; set; }
        public Site? Site { get; set; }

        public int? DepartmentId { get; set; }
        public Department? Department { get; set; }

        // Financial & Vendor
        public DateTime? PurchaseDate { get; set; }
        public decimal? PurchasePrice { get; set; }

        [MaxLength(200)]
        public string? Vendor { get; set; }

        public DateTime? WarrantyExpiration { get; set; }

        // Auditing
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public int? CreatedByEmployeeId { get; set; }
        public Employee? CreatedByEmployee { get; set; }

        // Navigation properties
        public ICollection<AssetAssignment> Assignments { get; set; } = new List<AssetAssignment>();
        public ICollection<AssetMaintenance> MaintenanceRecords { get; set; } = new List<AssetMaintenance>();
        public ICollection<AssetAuditLog> AuditLogs { get; set; } = new List<AssetAuditLog>();
    }
}
