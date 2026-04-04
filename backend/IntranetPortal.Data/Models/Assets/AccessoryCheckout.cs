using System;

namespace IntranetPortal.Data.Models.Assets
{
    public class AccessoryCheckout
    {
        public int Id { get; set; }

        public int AccessoryId { get; set; }
        public Accessory? Accessory { get; set; }

        public int RequestedByEmployeeId { get; set; }
        public Employee? RequestedByEmployee { get; set; }

        public int? FulfilledByEmployeeId { get; set; }
        public Employee? FulfilledByEmployee { get; set; }

        public int Quantity { get; set; } = 1;

        public DateTime CheckoutDate { get; set; } = DateTime.UtcNow;

        public CheckoutStatus Status { get; set; } = CheckoutStatus.Fulfilled;
    }
}
