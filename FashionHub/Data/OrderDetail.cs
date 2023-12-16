using System;
using System.Collections.Generic;

namespace FashionHub.Data
{
    public partial class OrderDetail
    {
        public string OrderDetailId { get; set; } = null!;
        public string? OrderId { get; set; }
        public string? ProductId { get; set; }
        public int? Quantity { get; set; }
        public decimal? Subtotal { get; set; }

        public virtual Order? Order { get; set; }
        public virtual Product? Product { get; set; }
    }
}
