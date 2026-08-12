using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DigitalMarketing.DigitalMarketing.Services.DTOs.ProductDtos
{
    public class CreateProductDto
    {
        public string Title { get; set; } = null!;
        public string ShortDescription { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal? Price { get; set; }
        public int ProductCategoryId { get; set; }
        public List<IFormFile> Images { get; set; } = new();
    }
}
