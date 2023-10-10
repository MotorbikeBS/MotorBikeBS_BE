﻿using Core.Models;

namespace API.DTO.BookingDTO
{
	public class BookingResponseRequestDTO
	{
		public int RequestId { get; set; }
		public int? MotorId { get; set; }
		public int? ReceiverId { get; set; }
		public int? SenderId { get; set; }
		public DateTime? Time { get; set; }
		public int? RequestTypeId { get; set; }
		public string? Status { get; set; }

		public virtual Motorbike? Motor { get; set; }
		public virtual ICollection<Booking> Bookings { get; set; }
	}
}
