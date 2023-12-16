using System;
using System.Collections.Generic;

namespace FashionHub.Data
{
    public partial class Review
    {
        public string ReviewId { get; set; } = null!;
        public string? UserId { get; set; }
        public string? ProductId { get; set; }
        public int? Rating { get; set; }
        public string? Comment { get; set; }

        public virtual Product? Product { get; set; }
        public virtual User? User { get; set; }
    }
}
