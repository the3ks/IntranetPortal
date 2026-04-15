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

        public RequestStatus Status { get; set; } = RequestStatus.PendingApproval;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation Property for Child Line Items
        public ICollection<AssetRequestLineItem> LineItems { get; set; } = new List<AssetRequestLineItem>();
    }
}
