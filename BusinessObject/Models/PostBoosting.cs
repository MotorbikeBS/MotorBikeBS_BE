using System;
using System.Collections.Generic;

namespace BusinessObject.Models
{
    public partial class PostBoosting
    {
        public int BoostId { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public int? Level { get; set; }
        public int MotorId { get; set; }
        public int? HistoryId { get; set; }
        public string? Status { get; set; }

        public virtual PointHistory? History { get; set; }
    }
}
