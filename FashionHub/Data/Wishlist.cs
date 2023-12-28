﻿namespace FashionHub.Data
{
    public partial class Wishlist
    {
        public string WishlistId { get; set; } = null!;
        public string? UserId { get; set; }
        public string? ProductId { get; set; }

        public virtual Product? Product { get; set; }
        public virtual User? User { get; set; }
    }
}
