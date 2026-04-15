using System.ComponentModel.DataAnnotations;

namespace IntranetPortal.Data.Models.Assets
{
    public class ApproverGroup
    {
        public int Id { get; set; }
        
        [Required]
        [MaxLength(100)]
        public required string Name { get; set; }
        
        public bool IsActive { get; set; } = true;

        public ICollection<ApproverGroupMember> Members { get; set; } = new List<ApproverGroupMember>();
        public ICollection<ApproverGroupScope> Scopes { get; set; } = new List<ApproverGroupScope>();
        public ICollection<AssetCategoryApproverGroup> AssetCategories { get; set; } = new List<AssetCategoryApproverGroup>();
    }
}
