using System;
using System.Collections.Generic;

namespace Core.Models
{
    public partial class MotorbikeModel
    {
        public MotorbikeModel()
        {
            Motorbikes = new HashSet<Motorbike>();
        }

        public int ModelId { get; set; }
        public string? ModelName { get; set; }
        public string? Description { get; set; }
        public string Status { get; set; } = null!;

        public virtual ICollection<Motorbike> Motorbikes { get; set; }
    }
}
