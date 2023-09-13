namespace API.DTO
{
	public class RegisterDTO
	{
		public string? UserName { get; set; }
		public string Email { get; set; } = null!;
		public string Password { get; set; } = null!;
		public string? Phone { get; set; }
		public int? Gender { get; set; }
		public DateTime? Dob { get; set; }
		public string? IdCard { get; set; }
		public string? Address { get; set; }
		public int? LocalId { get; set; }
		public int? RoleId { get; set; }
		public DateTime UserCreatedAt { get; set; }
		public DateTime UserUpdatedAt { get; set; }
		public string Status { get; set; } = null!;
	}
}
