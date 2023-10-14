using System;
using System.Collections.Generic;

namespace Core.Models
{
    public partial class ContractImage
    {
        public int ContractImageId { get; set; }
        public int ContractId { get; set; }
        public string? ImageLink { get; set; }
        public string? Description { get; set; }

        public virtual Contract Contract { get; set; } = null!;
    }
}
