using System;
using System.Collections.Generic;

namespace BusinessObject.Models
{
    public partial class User
    {
        public User()
        {
            Motorbikes = new HashSet<Motorbike>();
            RequestReceivers = new HashSet<Request>();
            RequestSenders = new HashSet<Request>();
            StoreDesciptionStoreManagers = new HashSet<StoreDesciption>();
            StoreDesciptionUsers = new HashSet<StoreDesciption>();
            Motors = new HashSet<Motorbike>();
        }

        public int UserId { get; set; }
        public string? UserName { get; set; }
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string? Phone { get; set; }
        public int? Gender { get; set; }
        public DateTime? Dob { get; set; }
        public string? IdCard { get; set; }
        public string? Address { get; set; }
        public int? LocalId { get; set; }
        public int? RoleId { get; set; }
        public DateTime UserCreatedAt { get; set; }
        public DateTime UserUpdatedAt { get; set; }
        public string Status { get; set; } = null!;

        public virtual LocalAddress? Local { get; set; }
        public virtual Role? Role { get; set; }
        public virtual ICollection<Motorbike> Motorbikes { get; set; }
        public virtual ICollection<Request> RequestReceivers { get; set; }
        public virtual ICollection<Request> RequestSenders { get; set; }
        public virtual ICollection<StoreDesciption> StoreDesciptionStoreManagers { get; set; }
        public virtual ICollection<StoreDesciption> StoreDesciptionUsers { get; set; }

        public virtual ICollection<Motorbike> Motors { get; set; }
    }
}
