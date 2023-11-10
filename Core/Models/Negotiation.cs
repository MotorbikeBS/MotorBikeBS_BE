using System;
using System.Collections.Generic;

namespace Core.Models
{
    public partial class Negotiation
    {
        public int NegotiationId { get; set; }
        public int? MotorId { get; set; }
        public decimal? FinalPrice { get; set; }
        public int? StoreId { get; set; }
        public string? Content { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? Status { get; set; }
        public int? ValuationId { get; set; }
        public int? BaseRequestId { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public decimal? Deposit { get; set; }

        public virtual Valuation? Valuation { get; set; }
    }
}
