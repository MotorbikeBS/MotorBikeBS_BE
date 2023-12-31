﻿using System;
using System.Collections.Generic;

namespace Core.Models
{
    public partial class StoreDescription
    {
        public StoreDescription()
        {
            Motorbikes = new HashSet<Motorbike>();
            StoreImages = new HashSet<StoreImage>();
        }

        public int StoreId { get; set; }
        public int UserId { get; set; }
        public string StoreName { get; set; } = null!;
        public string? Description { get; set; }
        public string? StorePhone { get; set; }
        public string? StoreEmail { get; set; }
        public DateTime? StoreCreatedAt { get; set; }
        public DateTime? StoreUpdatedAt { get; set; }
        public int? Point { get; set; }
        public string? Address { get; set; }
        public string? Status { get; set; }
        public string? TaxCode { get; set; }
        public string? BusinessLicense { get; set; }

        public virtual User User { get; set; } = null!;
        public virtual ICollection<Motorbike> Motorbikes { get; set; }
        public virtual ICollection<StoreImage> StoreImages { get; set; }
    }
}
