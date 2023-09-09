using System;
using System.Collections.Generic;

namespace BusinessObject.Models
{
    public partial class PointHistory
    {
        public PointHistory()
        {
            Payments = new HashSet<Payment>();
            PostBoostings = new HashSet<PostBoosting>();
        }

        public int PHistoryId { get; set; }
        public int RequestId { get; set; }
        public int? Qty { get; set; }
        public DateTime? PointUpdatedAt { get; set; }
        public string? Description { get; set; }
        public string? Action { get; set; }
        public int StoreId { get; set; }

        public virtual Request Request { get; set; } = null!;
        public virtual Request Store { get; set; } = null!;
        public virtual ICollection<Payment> Payments { get; set; }
        public virtual ICollection<PostBoosting> PostBoostings { get; set; }
    }
}
