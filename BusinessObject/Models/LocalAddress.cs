using System;
using System.Collections.Generic;

namespace BusinessObject.Models
{
    public partial class LocalAddress
    {
        public LocalAddress()
        {
            StoreDesciptions = new HashSet<StoreDesciption>();
            Users = new HashSet<User>();
        }

        public int LocalId { get; set; }
        public string WardName { get; set; } = null!;
        public string DistrictName { get; set; } = null!;
        public string CityName { get; set; } = null!;

        public virtual ICollection<StoreDesciption> StoreDesciptions { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}
