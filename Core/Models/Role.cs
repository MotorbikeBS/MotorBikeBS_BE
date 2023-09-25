using System;
using System.Collections.Generic;

namespace Core.Models
{
    public partial class Role
    {
        public Role()
        {
            Users = new HashSet<User>();
        }

        public int RoleId { get; set; }
        public string Title { get; set; } = null!;

        public virtual ICollection<User> Users { get; set; }
    }
}
