using API.DTO.Role;
using API.DTO.StoreDTO;
using Core.Models;

namespace API.DTO.UserDTO
{
	public class UserResponseDTO
	{
		public int UserId { get; set; }
		public string? UserName { get; set; }
		public string Email { get; set; } = null!;
		public string? Phone { get; set; }
		public int? Gender { get; set; }
		public DateTime? Dob { get; set; }
		public string? IdCard { get; set; }
		public string? Address { get; set; }
		public int? LocalId { get; set; }
		public int? RoleId { get; set; }
		public DateTime? UserVerifyAt { get; set; }
		public DateTime? UserUpdatedAt { get; set; }
		public string Status { get; set; } = null!;
		public virtual RoleResponseDTO Role { get; set; }
		public virtual ICollection<StoreDescriptionResponseDTO> StoreDesciptions { get; set; }
	}
}
