namespace Core.Models
{
    public partial class BrandRegisterDTO
    {
        public int BrandId { get; set; }
        public string? BrandName { get; set; }
        public string? Description { get; set; }
        public string Status { get; set; } = null!;
    }
}
