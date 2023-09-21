using System;
using System.Collections.Generic;

namespace Core.Models
{
    public partial class Motorbike
    {
        public Motorbike()
        {
            MotorbikeImages = new HashSet<MotorbikeImage>();
            Requests = new HashSet<Request>();
            Users = new HashSet<User>();
        }

        public int MotorId { get; set; }
        public string? Brand { get; set; }
        public string? Model { get; set; }
        public int? Odo { get; set; }
        public DateTime? Year { get; set; }
        public decimal? Price { get; set; }
        public string? Description { get; set; }
        public int? MotorStatusId { get; set; }
        public int? MotorTypeId { get; set; }
        public int? StoreId { get; set; }
        public int OwnerId { get; set; }

        public virtual ICollection<MotorbikeImage> MotorbikeImages { get; set; }
        public virtual ICollection<Request> Requests { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }
}
