using System;
using System.Collections.Generic;

namespace BusinessObject.Models
{
    public partial class EarnAlivingContract
    {
        public EarnAlivingContract()
        {
            EarnAlivingContractImages = new HashSet<EarnAlivingContractImage>();
        }

        public int ContractId { get; set; }
        public int? MotorId { get; set; }
        public decimal? Price { get; set; }
        public int? NewOwner { get; set; }
        public int? StoreId { get; set; }
        public string? Content { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? Status { get; set; }
        public int? BookingId { get; set; }
        public int? NegotiationId { get; set; }

        public virtual Booking? Booking { get; set; }
        public virtual Negotiation? Negotiation { get; set; }
        public virtual ICollection<EarnAlivingContractImage> EarnAlivingContractImages { get; set; }
    }
}
