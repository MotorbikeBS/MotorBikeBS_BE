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
            Wishlists = new HashSet<Wishlist>();
        }

        public int MotorId { get; set; }
        public string CertificateNumber { get; set; } = null!;
        public string? MotorName { get; set; }
        public int? ModelId { get; set; }
        public int? Odo { get; set; }
        public DateTime? Year { get; set; }
        public decimal? Price { get; set; }
        public string? Description { get; set; }
        public int? MotorStatusId { get; set; }
        public int? MotorTypeId { get; set; }
        public int? StoreId { get; set; }
        public int OwnerId { get; set; }

        public virtual MotorbikeModel? Model { get; set; }
        public virtual MotorbikeStatus? MotorStatus { get; set; }
        public virtual MotorbikeType? MotorType { get; set; }
        public virtual User Owner { get; set; } = null!;
        public virtual StoreDesciption? Store { get; set; }
        public virtual ICollection<MotorbikeImage> MotorbikeImages { get; set; }
        public virtual ICollection<Request> Requests { get; set; }
        public virtual ICollection<Wishlist> Wishlists { get; set; }
    }
}
