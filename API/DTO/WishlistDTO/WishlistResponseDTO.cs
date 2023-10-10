using API.DTO.MotorbikeDTO;
using API.DTO.UserDTO;
using Core.Models;
using Service.Repository;

namespace API.DTO.WishlistDTO
{
	public class WishlistResponseDTO
	{
		public int WishlistId { get; set; }
		public int UserId { get; set; }
		public int MotorId { get; set; }
		public string? MotorName { get; set; }

		public virtual MotorResponseDTO Motor { get; set; } = null!;
	}
}
