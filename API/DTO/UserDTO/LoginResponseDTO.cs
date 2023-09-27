namespace API.DTO.UserDTO
{
	public class LoginResponseDTO
	{
		public int UserId { get; set; }
		public string? UserName { get; set; }
		public string Email { get; set; } = null!;
		public string? Phone { get; set; }
		public int? RoleId { get; set; }
		public string? Token { get; set; }
	}
}
