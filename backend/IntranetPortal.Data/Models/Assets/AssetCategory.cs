using System.ComponentModel.DataAnnotations;

namespace IntranetPortal.Data.Models.Assets
{
    public class AssetCategory
    {
        public int Id { get; set; }
        
        [Required]
        [MaxLength(100)]
        public required string Name { get; set; }
        
        [MaxLength(500)]
        public string? Description { get; set; }

        public int? ParentCategoryId { get; set; }
        public AssetCategory? ParentCategory { get; set; }

        public bool RequiresApproval { get; set; } = true;
        public bool IsActive { get; set; } = true;

        public bool AllowRequesterToSelectApprover { get; set; } = false;
        public int? DefaultApproverGroupId { get; set; }
        public ApproverGroup? DefaultApproverGroup { get; set; }

        public int? FulfillmentGroupId { get; set; }
        public ApproverGroup? FulfillmentGroup { get; set; }

        // Navigation properties
        public ICollection<AssetCategory> SubCategories { get; set; } = new List<AssetCategory>();
        public ICollection<AssetModel> Models { get; set; } = new List<AssetModel>();
        public ICollection<Accessory> Accessories { get; set; } = new List<Accessory>();
        public ICollection<AssetCategoryApproverGroup> ApproverGroups { get; set; } = new List<AssetCategoryApproverGroup>();
    }
}
