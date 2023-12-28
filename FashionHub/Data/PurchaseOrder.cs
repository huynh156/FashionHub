namespace FashionHub.Data
{
    public partial class PurchaseOrder
    {
        public string OrderId { get; set; } = null!;
        public string? ProductId { get; set; }
        public int? Quantity { get; set; }
        public DateTime? OrderDate { get; set; }
        public string? BrandId { get; set; }
        public string Id { get; set; } = null!;

        public virtual Brand? Brand { get; set; }
        public virtual Product? Product { get; set; }
    }
}
