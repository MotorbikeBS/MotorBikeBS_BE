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
            Facilities = new HashSet<Facility>();
            Users = new HashSet<User>();
        }

        public int MotorId { get; set; }
        public string? Brand { get; set; }
        public string? Model { get; set; }
        public DateTime? Year { get; set; }
        public decimal? Price { get; set; }
        public string? Description { get; set; }
        public int? MotorStatusId { get; set; }
        public int? MotorTypeId { get; set; }
        public int? ImageId { get; set; }
        public int? StoreId { get; set; }
        public int OwnerId { get; set; }
        public int? Odo { get; set; }

        public virtual MotorbikeStatus? MotorStatus { get; set; }
        public virtual MotorbikeType? MotorType { get; set; }
        public virtual User Owner { get; set; } = null!;
        public virtual ICollection<MotorbikeImage> MotorbikeImages { get; set; }
        public virtual ICollection<Request> Requests { get; set; }

        public virtual ICollection<Facility> Facilities { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}
