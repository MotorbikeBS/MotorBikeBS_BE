﻿using API.DTO.StoreDTO;
using API.DTO.UserDTO;
using Core.Models;

namespace API.DTO.NegotiationDTO
{
	public class ValuationResponseRequestDTO
	{
		public int RequestId { get; set; }
		public int? MotorId { get; set; }
		public int? ReceiverId { get; set; }
		public int? SenderId { get; set; }
		public DateTime? Time { get; set; }
		public int? RequestTypeId { get; set; }
		public string? Status { get; set; }

		public virtual Motorbike? Motor { get; set; }
		public virtual ICollection<ValuationResponseDTO> Valuations { get; set; }
		public virtual UserResponseDTO? Receiver { get; set; }
		public virtual UserResponseDTO? Sender { get; set; }
	}
}
