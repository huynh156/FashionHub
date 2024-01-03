namespace FashionHub.ViewModels
{
    public class OrdersVM
    {
        public string OrderId { get; set; }
        public string TotalAmount { get; set; }
        public string OrderDate { get; set; }
        public string ExpectedDeliveryDate { get; set; }
        public string ReceivedDate { get; set; }       
        public string FullName { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string PaymentMethod { get; set; }
        public string StatusName { get; set; }
        public string ShippingMethod { get; set; }
    }
}
