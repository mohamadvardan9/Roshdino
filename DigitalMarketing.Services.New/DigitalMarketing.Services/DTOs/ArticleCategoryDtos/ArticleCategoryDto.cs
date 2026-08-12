namespace DigitalMarketing.DigitalMarketing.Services.DTOs.ArticleCategoryDtos
{
    public class ArticleCategoryDto
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Slug { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }
    }
}
