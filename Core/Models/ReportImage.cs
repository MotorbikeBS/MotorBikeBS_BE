using System;
using System.Collections.Generic;

namespace Core.Models
{
    public partial class ReportImage
    {
        public int ReportImageId { get; set; }
        public int ReportId { get; set; }
        public string ImageLink { get; set; } = null!;

        public virtual Report Report { get; set; } = null!;
    }
}
