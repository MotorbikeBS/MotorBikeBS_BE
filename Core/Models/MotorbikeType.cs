using System;
using System.Collections.Generic;

namespace Core.Models
{
    public partial class MotorbikeType
    {
        public MotorbikeType()
        {
            Motorbikes = new HashSet<Motorbike>();
        }

        public int MotorTypeId { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Status { get; set; }

        public virtual ICollection<Motorbike> Motorbikes { get; set; }
    }
}
