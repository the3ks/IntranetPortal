namespace IntranetPortal.Data.Models.Assets
{
    public class AssetCategoryApproverGroup
    {
        public int AssetCategoryId { get; set; }
        public AssetCategory? AssetCategory { get; set; }

        public int ApproverGroupId { get; set; }
        public ApproverGroup? ApproverGroup { get; set; }
    }
}
