namespace API.DTO.ContractDTO
{
    public class ValuationRequestResponseDTO
    {
        public int ValueationId { get; set; }
        public int RequestId { get; set; }
        public decimal? StorePrice { get; set; }
        public string? Description { get; set; }
        public string? Status { get; set; }

        public virtual ICollection<NegotiationResponseDTO> Negotiations { get; set; }
    }
}
