namespace Core.Models
{
    public partial class BrandRegisterDTO
    {
        public string? BrandName { get; set; }
        public string? Description { get; set; }
        public string Status { get; set; } = null!;
    }
}
