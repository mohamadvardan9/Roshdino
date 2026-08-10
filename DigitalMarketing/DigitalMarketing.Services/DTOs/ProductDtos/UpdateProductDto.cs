namespace DigitalMarketing.DigitalMarketing.Services.DTOs.ProductDtos
{
    public class UpdateProductDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string ShortDescription { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal? Price { get; set; }
        public int ProductCategoryId { get; set; }
        public bool IsPublished { get; set; }


        
        // تصاویر فعلی محصول در دیتابیس
        public List<ProductImageDto> Images { get; set; } = new();

        // فایل های تصویری جدیدی که کابر برای اضافه کردن اتخاب کرده 
        public List<IFormFile> NewImages { get; set; } = new();
    }
}
