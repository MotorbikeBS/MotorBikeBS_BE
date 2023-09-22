namespace Core.Models
{
    public partial class ConsignmentContractImage
    {
        public int ContractImageId { get; set; }
        public int ContractId { get; set; }
        public string? ImageLink { get; set; }
        public string? Description { get; set; }

        public virtual ConsignmentContract Contract { get; set; } = null!;
    }
}
