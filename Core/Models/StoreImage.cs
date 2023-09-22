namespace Core.Models
{
    public partial class StoreImage
    {
        public int StoreImageId { get; set; }
        public string ImageLink { get; set; } = null!;
        public int StoreId { get; set; }

        public virtual StoreDesciption Store { get; set; } = null!;
    }
}
