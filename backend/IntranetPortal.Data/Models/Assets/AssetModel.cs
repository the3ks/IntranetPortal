using System.ComponentModel.DataAnnotations;

namespace IntranetPortal.Data.Models.Assets
{
    public class AssetModel
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public required string Manufacturer { get; set; }

        [Required]
        [MaxLength(200)]
        public required string Name { get; set; }

        public int CategoryId { get; set; }
        public AssetCategory? Category { get; set; }

        public bool IsActive { get; set; } = true;

        // Navigation property
        public ICollection<Asset> Assets { get; set; } = new List<Asset>();
    }
}
