namespace Core.Models
{
    public partial class ModelResponseDTO
    {
        public int ModelId { get; set; }
        public string? ModelName { get; set; }
        public string? Description { get; set; }
        public string Status { get; set; } = null!;
        public virtual BrandRegisterDTO? Brand { get; set; }
    }
}
