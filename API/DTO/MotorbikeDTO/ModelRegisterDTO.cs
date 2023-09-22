namespace Core.Models
{
    public partial class ModelRegisterDTO
    {

        public int ModelId { get; set; }
        public string? ModelName { get; set; }
        public string? Description { get; set; }
        public string Status { get; set; } = null!;
    }
}
