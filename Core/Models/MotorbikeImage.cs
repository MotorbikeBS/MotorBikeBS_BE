using System;
using System.Collections.Generic;

namespace Core.Models
{
    public partial class MotorbikeImage
    {
        public int ImageId { get; set; }
        public string? ImageLink { get; set; }
        public int? MotorId { get; set; }

        public virtual Motorbike? Motor { get; set; }
    }
}
