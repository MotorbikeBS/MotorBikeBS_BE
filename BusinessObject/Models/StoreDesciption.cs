using System;
using System.Collections.Generic;

namespace BusinessObject.Models
{
    public partial class StoreDesciption
    {
        public int StoreId { get; set; }
        public int UserId { get; set; }
        public string StoreName { get; set; } = null!;
        public string? Description { get; set; }
        public string? StorePhone { get; set; }
        public string? StoreEmail { get; set; }
        public DateTime? StoreCreatedAt { get; set; }
        public DateTime? StoreUpdatedAt { get; set; }
        public int StoreManagerId { get; set; }
        public int? Point { get; set; }
        public string? Address { get; set; }
        public int? LocalId { get; set; }

        public virtual LocalAddress? Local { get; set; }
        public virtual User StoreManager { get; set; } = null!;
        public virtual User User { get; set; } = null!;
    }
}
