using System;
using System.ComponentModel.DataAnnotations;

namespace IntranetPortal.Data.Models.Assets
{
    public class AssetRequest
    {
        public int Id { get; set; }

        public int RequestedByEmployeeId { get; set; }
        public Employee? RequestedByEmployee { get; set; }

        public int RequestedForEmployeeId { get; set; }
        public Employee? RequestedForEmployee { get; set; }

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

        public DateTime? ManagerApprovedAt { get; set; }
        public int? ManagerApprovedByEmployeeId { get; set; }
        public Employee? ManagerApprovedByEmployee { get; set; }

        public DateTime? FulfilledAt { get; set; }
        public int? FulfilledByEmployeeId { get; set; }
        public Employee? FulfilledByEmployee { get; set; }

        // The specific serialized asset that was provided (if applicable)
        public int? AssignedAssetId { get; set; }
        public Asset? AssignedAsset { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
