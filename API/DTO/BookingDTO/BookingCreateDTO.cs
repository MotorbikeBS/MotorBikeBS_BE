namespace API.DTO.BookingDTO
{
	public class BookingCreateDTO
	{
		public int? MotorId { get; set; }
		public DateTime? BookingDate { get; set; }
		public string? Note { get; set; }
	}
}
