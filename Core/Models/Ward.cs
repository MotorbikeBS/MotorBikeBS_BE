using System;
using System.Collections.Generic;

namespace Core.Models
{
    public partial class Ward
    {
        public Ward()
        {
            StoreDesciptions = new HashSet<StoreDesciption>();
            Users = new HashSet<User>();
        }

        public string WardId { get; set; } = null!;
        public string WardName { get; set; } = null!;
        public string? Type { get; set; }
        public string? DistrictId { get; set; }

        public virtual ICollection<StoreDesciption> StoreDesciptions { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}
