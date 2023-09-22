namespace Core.Models
{
    public partial class ConsignmentContract
    {
        public ConsignmentContract()
        {
            ConsignmentContractImages = new HashSet<ConsignmentContractImage>();
        }

        public int ContractId { get; set; }
        public int? MotorId { get; set; }
        public decimal? Price { get; set; }
        public int? NewOwner { get; set; }
        public int? StoreId { get; set; }
        public string? Content { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? Status { get; set; }
        public int? NegotiationId { get; set; }

        public virtual Negotiation? Negotiation { get; set; }
        public virtual ICollection<ConsignmentContractImage> ConsignmentContractImages { get; set; }
    }
}
