using System;
using System.Collections.Generic;

namespace Core.Models
{
    public partial class User
    {
        public User()
        {
            Motorbikes = new HashSet<Motorbike>();
            RequestReceivers = new HashSet<Request>();
            RequestSenders = new HashSet<Request>();
            StoreDesciptions = new HashSet<StoreDesciption>();
            Wishlists = new HashSet<Wishlist>();
        }

        public int UserId { get; set; }
        public string? UserName { get; set; }
        public string Email { get; set; } = null!;
        public string? Phone { get; set; }
        public int? Gender { get; set; }
        public DateTime? Dob { get; set; }
        public string? IdCard { get; set; }
        public string? Address { get; set; }
        public string? WardId { get; set; }
        public int? RoleId { get; set; }
        public DateTime? UserVerifyAt { get; set; }
        public DateTime? UserUpdatedAt { get; set; }
        public string Status { get; set; } = null!;
        public byte[]? PasswordHash { get; set; }
        public byte[]? PasswordSalt { get; set; }
        public string? PasswordResetToken { get; set; }
        public DateTime? ResetTokenExpires { get; set; }
        public string? VerifycationToken { get; set; }
        public DateTime? VerifycationTokenExpires { get; set; }

        public virtual Role? Role { get; set; }
        public virtual Ward? Ward { get; set; }
        public virtual ICollection<Motorbike> Motorbikes { get; set; }
        public virtual ICollection<Request> RequestReceivers { get; set; }
        public virtual ICollection<Request> RequestSenders { get; set; }
        public virtual ICollection<StoreDesciption> StoreDesciptions { get; set; }
        public virtual ICollection<Wishlist> Wishlists { get; set; }
    }
}
