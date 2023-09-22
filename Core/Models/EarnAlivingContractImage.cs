namespace Core.Models
{
    public partial class EarnAlivingContractImage
    {
        public int ContractImageId { get; set; }
        public int ContractId { get; set; }
        public string? ImageLink { get; set; }
        public string? Description { get; set; }

        public virtual EarnAlivingContract Contract { get; set; } = null!;
    }
}
