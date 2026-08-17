namespace DigitalMarketing.DigitalMarketing.Services.DTOs.ProductDtos
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Slug { get; set; } = null!;
        public string ShortDescription { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal? Price { get; set; }
        public bool IsPublished { get; set; }
        public DateTime CreatedAt { get; set; }


        public int ProductCategoryId { get; set; }
        public string ProductCategoryNAme { get; set; } = null!;

        public List<ProductImageDto> Images { get; set; } = new();
    }
}
