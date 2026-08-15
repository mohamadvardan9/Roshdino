using DigitalMarketing.DigitalMarketing.Services.DTOs.ProductCategoryDtos;
using DigitalMarketing.DigitalMarketing.Services.DTOs.ProductDtos;

namespace DigitalMarketing.Web.Models
{
    public class ProductListViewModel
    {
        public List<ProductDto> Products { get; set; } = new();
        public List<ProductCategoryDto> Categories { get; set; } = new();

        // برای نمایش اینکه الان کدوم دسته‌بندی فیلتر شده (برای هایلایت تو یو آی)تمام ;)
        public int? SelectedCategoryId { get; set; }
    }
}
