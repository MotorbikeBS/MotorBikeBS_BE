using System;
using System.Collections.Generic;

namespace Core.Models
{
    public partial class MotorbikeBrand
    {
        public MotorbikeBrand()
        {
            Motorbikes = new HashSet<Motorbike>();
        }

        public int BrandId { get; set; }
        public string? BrandName { get; set; }
        public string? Description { get; set; }
        public string Status { get; set; } = null!;

        public virtual ICollection<Motorbike> Motorbikes { get; set; }
    }
}
