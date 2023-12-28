namespace FashionHub.Data
{
    public partial class Product
    {
        public Product()
        {
            Coupons = new HashSet<Coupon>();
            OrderDetails = new HashSet<OrderDetail>();
            PurchaseOrders = new HashSet<PurchaseOrder>();
            Reviews = new HashSet<Review>();
            Wishlists = new HashSet<Wishlist>();
        }

        public string ProductId { get; set; } = null!;
        public string? ProductName { get; set; }
        public string? BrandId { get; set; }
        public string? CategoryId { get; set; }
        public string? Description { get; set; }
        public decimal? Price { get; set; }
        public int? StockQuantity { get; set; }
        public string? Image { get; set; }
        public string? CouponId { get; set; }

        public virtual Brand? Brand { get; set; }
        public virtual Category? Category { get; set; }
        public virtual Coupon? Coupon { get; set; }
        public virtual ICollection<Coupon> Coupons { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
        public virtual ICollection<PurchaseOrder> PurchaseOrders { get; set; }
        public virtual ICollection<Review> Reviews { get; set; }
        public virtual ICollection<Wishlist> Wishlists { get; set; }
    }
}
