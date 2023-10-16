using System;
using System.Collections.Generic;

namespace Core.Models
{
    public partial class Negotiation
    {
        public Negotiation()
        {
            Bookings = new HashSet<Booking>();
        }

        public int NegotiationId { get; set; }
        public int RequestId { get; set; }
        public decimal? StorePrice { get; set; }
        public decimal? OwnerPrice { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public string? Description { get; set; }
        public string? Status { get; set; }
        public decimal? FinalPrice { get; set; }
        public int? BaseRequestId { get; set; }

        public virtual Request Request { get; set; } = null!;
        public virtual ICollection<Booking> Bookings { get; set; }
    }
}
