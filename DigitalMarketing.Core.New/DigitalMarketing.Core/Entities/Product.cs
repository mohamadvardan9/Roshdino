namespace DigitalMarketing.DigitalMarketing.Core.Entities
{
    public class Product : BaseEntity
    {
        public string Title { get; set; } = null!;
        public string Slug { get; set; } = null!;  // شناسه متنی خوانا و مناسب مثل : Apple Iphone 16 Pro Max
        public string ShortDescription { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal? Price { get; set; }
        public bool IsPublished { get; set; } = true;

        public int ProductCategoryId { get; set; }
        public ProductCategory ProductCategory { get; set; } = null!;

        public ICollection<ProductImage> Images { get; set; } = new List<ProductImage>();
    }
}
