using System;
using System.Collections.Generic;

namespace Core.Models
{
    public partial class Booking
    {
        public Booking()
        {
            Contracts = new HashSet<Contract>();
        }

        public int BookingId { get; set; }
        public int? NegotiationId { get; set; }
        public DateTime? DateCreate { get; set; }
        public DateTime? BookingDate { get; set; }
        public string? Note { get; set; }
        public string? Status { get; set; }
        public int? BaseRequestId { get; set; }

        public virtual Negotiation? Negotiation { get; set; }
        public virtual ICollection<Contract> Contracts { get; set; }
    }
}
