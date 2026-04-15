using System;
using System.ComponentModel.DataAnnotations;

namespace IntranetPortal.Data.Models.Assets
{
    public class AssetRequestLineItem
    {
        public int Id { get; set; }

        public int AssetRequestId { get; set; }
        public AssetRequest? AssetRequest { get; set; }

        public RequestType Type { get; set; }

        public int? RequestedCategoryId { get; set; }
        public AssetCategory? RequestedCategory { get; set; }

        public int? RequestedModelId { get; set; }
        public AssetModel? RequestedModel { get; set; }

        public int? RequestedAccessoryId { get; set; }
        public Accessory? RequestedAccessory { get; set; }

        public int Quantity { get; set; } = 1;

        [Required]
        [MaxLength(2000)]
        public required string Justification { get; set; }

        public RequestStatus Status { get; set; } = RequestStatus.PendingApproval;

        // Routing specific to this line item
        public int? SelectedApproverEmployeeId { get; set; }
        public Employee? SelectedApproverEmployee { get; set; }

        public int? AssignedApproverGroupId { get; set; }
        public ApproverGroup? AssignedApproverGroup { get; set; }

        public DateTime? ApprovedAt { get; set; }
        public int? ApprovedByEmployeeId { get; set; }
        public Employee? ApprovedByEmployee { get; set; }

        public DateTime? FulfilledAt { get; set; }
        public int? FulfilledByEmployeeId { get; set; }
        public Employee? FulfilledByEmployee { get; set; }

        // The specific serialized asset that was provided (if applicable)
        public int? AssignedAssetId { get; set; }
        public Asset? AssignedAsset { get; set; }
    }
}
