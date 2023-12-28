namespace FashionHub.Data
{
    public partial class Coupon
    {
        public Coupon()
        {
            Orders = new HashSet<Order>();
            Products = new HashSet<Product>();
        }

        public string CouponId { get; set; } = null!;
        public string? Code { get; set; }
        public decimal? DiscountPercentage { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public bool? IsActive { get; set; }
        public int? Quantity { get; set; }
        public string? ProductId { get; set; }

        public virtual Product? Product { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }
}
