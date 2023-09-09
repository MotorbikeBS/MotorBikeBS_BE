using System;
using System.Collections.Generic;

namespace BusinessObject.Models
{
    public partial class Facility
    {
        public Facility()
        {
            Motors = new HashSet<Motorbike>();
        }

        public int FacilityId { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Status { get; set; }

        public virtual ICollection<Motorbike> Motors { get; set; }
    }
}
