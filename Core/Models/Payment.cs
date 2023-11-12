using System;
using System.Collections.Generic;

namespace Core.Models
{
    public partial class Payment
    {
        public int PaymentId { get; set; }
        public int RequestId { get; set; }
        public string? Content { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? PaymentTime { get; set; }
        public string? VnpayOrderId { get; set; }
        public string? PaymentType { get; set; }

        public virtual Request Request { get; set; } = null!;
    }
}
