using System;
using System.Collections.Generic;

namespace Core.Models
{
    public partial class Valuation
    {
        public Valuation()
        {
            Negotiations = new HashSet<Negotiation>();
        }

        public int ValueationId { get; set; }
        public int RequestId { get; set; }
        public decimal? StorePrice { get; set; }
        public string? Description { get; set; }
        public string? Status { get; set; }

        public virtual Request Request { get; set; } = null!;
        public virtual ICollection<Negotiation> Negotiations { get; set; }
    }
}
