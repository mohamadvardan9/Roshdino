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


        // new added
        public List<ProductImageDto> Images { get; set; } = new();

        // عکس‌های جدیدی که تو همین ویرایش اضافه شدن (اختیاری)
        public List<string> NewImagePaths { get; set; } = new();
    }
}
