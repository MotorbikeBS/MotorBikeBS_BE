using System;
using System.Collections.Generic;

namespace Core.Models
{
    public partial class Report
    {
        public Report()
        {
            ReportImages = new HashSet<ReportImage>();
        }

        public int ReportId { get; set; }
        public int RequestId { get; set; }
        public string? Description { get; set; }
        public string? Title { get; set; }
        public string? Status { get; set; }

        public virtual Request Request { get; set; } = null!;
        public virtual ICollection<ReportImage> ReportImages { get; set; }
    }
}
