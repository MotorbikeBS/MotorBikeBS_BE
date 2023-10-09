using System;
using System.Collections.Generic;

namespace Core.Models
{
    public partial class Wishlist
    {
        public int WishlistId { get; set; }
        public int UserId { get; set; }
        public int MotorId { get; set; }
        public string? MotorName { get; set; }

        public virtual Motorbike Motor { get; set; } = null!;
        public virtual User User { get; set; } = null!;
    }
}
