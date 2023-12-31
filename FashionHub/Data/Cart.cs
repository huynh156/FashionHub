﻿using System;
using System.Collections.Generic;

namespace FashionHub.Data
{
    public partial class Cart
    {
        public string? UserId { get; set; }
        public string? ProductId { get; set; }
        public int? Quantity { get; set; }

        public virtual Product? Product { get; set; }
        public virtual User? User { get; set; }
    }
}
