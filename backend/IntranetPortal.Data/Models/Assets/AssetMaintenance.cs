using System;
using System.ComponentModel.DataAnnotations;

namespace IntranetPortal.Data.Models.Assets
{
    public class AssetMaintenance
    {
        public int Id { get; set; }

        public int AssetId { get; set; }
        public Asset? Asset { get; set; }

        public DateTime MaintenanceDate { get; set; }

        [Required]
        [MaxLength(1000)]
        public required string Description { get; set; }

        public decimal? Cost { get; set; }

        [MaxLength(200)]
        public string? RepairVendor { get; set; }

        public int LoggedByEmployeeId { get; set; }
        public Employee? LoggedByEmployee { get; set; }
    }
}
