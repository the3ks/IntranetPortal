using System.ComponentModel.DataAnnotations;

namespace IntranetPortal.Data.Models.Assets
{
    public class Accessory : ISiteScoped, IDepartmentScoped
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public required string Name { get; set; }

        public int CategoryId { get; set; }
        public AssetCategory? Category { get; set; }

        public int TotalQuantity { get; set; }
        public int AvailableQuantity { get; set; }

        public int? MinStockThreshold { get; set; }

        // RBAC Boundaries
        public int? SiteId { get; set; }
        public Site? Site { get; set; }

        public int? DepartmentId { get; set; }
        public Department? Department { get; set; }

        // Navigation
        public ICollection<AccessoryCheckout> Checkouts { get; set; } = new List<AccessoryCheckout>();
    }
}
