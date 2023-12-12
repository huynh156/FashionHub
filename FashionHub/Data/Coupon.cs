using System;
using System.Collections.Generic;

namespace FashionHub.Data
{
    public partial class Coupon
    {
        public string CouponId { get; set; } = null!;
        public string? Code { get; set; }
        public decimal? DiscountPercentage { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public bool? IsActive { get; set; }
    }
}
