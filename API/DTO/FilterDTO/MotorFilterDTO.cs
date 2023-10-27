using Core.Models;

namespace API.DTO.FilterDTO
{
    public partial class MotorFilterDTO
    {
        public List<int>? BrandId { get; set; }
        public List<int>? ModelId { get; set; }
        public decimal? minPrice { get; set; } 
        public decimal? maxPrice { get; set; }
        public List<int>? MotorTypeId { get; set; }

    }
}

