using Core.Models;

namespace API.DTO.BookingNegotiationDTO
{
	public class NegotiationRequestResponseDTO
	{
		public int NegotiationId { get; set; }
		public int RequestId { get; set; }
		public decimal? StorePrice { get; set; }
		public decimal? OwnerPrice { get; set; }
		public DateTime? StartTime { get; set; }
		public DateTime? EndTime { get; set; }
		public string? Description { get; set; }
		public string? Status { get; set; }
		public decimal? FinalPrice { get; set; }
		public int? BaseRequestId { get; set; }
		public DateTime? ExpiredTime { get; set; }
		public int? LastChangeUserId { get; set; }

		public virtual ICollection<BookingNegoResponseDTO> Bookings { get; set; }
	}
}
