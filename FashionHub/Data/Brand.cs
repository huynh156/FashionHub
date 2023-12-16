using System;
using System.Collections.Generic;

namespace FashionHub.Data
{
    public partial class Brand
    {
        public Brand()
        {
            Products = new HashSet<Product>();
        }

        public string BrandId { get; set; } = null!;
        public string? BrandName { get; set; }
        public string? Description { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}
