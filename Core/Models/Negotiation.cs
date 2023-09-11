using System;
using System.Collections.Generic;

namespace Core.Models
{
    public partial class Negotiation
    {
        public Negotiation()
        {
            ConsignmentContracts = new HashSet<ConsignmentContract>();
            EarnAlivingContracts = new HashSet<EarnAlivingContract>();
        }

        public int NegotiationId { get; set; }
        public int RequestId { get; set; }
        public decimal? Price { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public string? Description { get; set; }
        public string? Status { get; set; }
        public bool? FromSeller { get; set; }

        public virtual Request Request { get; set; } = null!;
        public virtual ICollection<ConsignmentContract> ConsignmentContracts { get; set; }
        public virtual ICollection<EarnAlivingContract> EarnAlivingContracts { get; set; }
    }
}
