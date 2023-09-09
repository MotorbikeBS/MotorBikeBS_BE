using System;
using System.Collections.Generic;

namespace BusinessObject.Models
{
    public partial class Payment
    {
        public int PaymentId { get; set; }
        public int HistoryId { get; set; }
        public string? Content { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? PaymentTime { get; set; }
        public string? PaymentType { get; set; }

        public virtual PointHistory History { get; set; } = null!;
    }
}
