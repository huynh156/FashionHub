using System;
using System.Collections.Generic;

namespace FashionHub.Data
{
    public partial class Order
    {
        public Order()
        {
            OrderDetails = new HashSet<OrderDetail>();
        }

        public string OrderId { get; set; } = null!;
        public string? UserId { get; set; }
        public DateTime? OrderDate { get; set; }
        public decimal? TotalAmount { get; set; }
        public string? CouponId { get; set; }
        public DateTime? ReceivedDate { get; set; }
        public DateTime? ExpectedDeliveryDate { get; set; }
        public string? FullName { get; set; }
        public string? Address { get; set; }
        public string? PaymentMethod { get; set; }
        public int? StatusId { get; set; }
        public int? ShipperId { get; set; }
        public string? ShippingMethod { get; set; }
        public string? Notes { get; set; }
        public string? PhoneNumber { get; set; }

        public virtual Coupon? Coupon { get; set; }
        public virtual Shipper? Shipper { get; set; }
        public virtual Status? Status { get; set; }
        public virtual User? User { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
