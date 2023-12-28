namespace FashionHub.Data
{
    public partial class Brand
    {
        public Brand()
        {
            Products = new HashSet<Product>();
            PurchaseOrders = new HashSet<PurchaseOrder>();
        }

        public string BrandId { get; set; } = null!;
        public string? BrandName { get; set; }
        public string? Description { get; set; }

        public virtual ICollection<Product> Products { get; set; }
        public virtual ICollection<PurchaseOrder> PurchaseOrders { get; set; }
    }
}
