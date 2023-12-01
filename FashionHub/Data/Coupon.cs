namespace FashionHub.Data
{
    public partial class Coupon
    {
        public int CouponId { get; set; }
        public string? Code { get; set; }
        public decimal? DiscountPercentage { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public bool? IsActive { get; set; }
    }
}
