namespace FashionHub.ViewModels
{
    public class CartItem
    {
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public string Image { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal SubTotal { get; set; }

    }
}
