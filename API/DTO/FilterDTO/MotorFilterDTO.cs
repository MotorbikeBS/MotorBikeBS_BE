using Core.Models;

namespace API.DTO.FilterDTO
{
    public partial class MotorFilterDTO
    {
        //public int[]? BrandId { get; set; }
        public List<int>? ModelId { get; set; }
        public decimal? minPrice { get; set; } 
        public decimal? maxPrice { get; set; }
        public int? MotorTypeId { get; set; }

    }
}

