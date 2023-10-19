using System;
using System.Collections.Generic;

namespace Core.Models
{
    public partial class BuyerBooking
    {
        public int BookingId { get; set; }
        public int RequestId { get; set; }
        public DateTime? DateCreate { get; set; }
        public DateTime? BookingDate { get; set; }
        public string? Note { get; set; }
        public string? Status { get; set; }

        public virtual Request Request { get; set; } = null!;
    }
}
