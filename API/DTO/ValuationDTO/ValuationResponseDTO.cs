﻿namespace API.DTO.NegotiationDTO
{
	public class ValuationResponseDTO
	{
        public int ValuationId { get; set; }
        public int RequestId { get; set; }
        public decimal? StorePrice { get; set; }
        public string? Description { get; set; }
        public string? Status { get; set; }
    }
}
