namespace DigitalMarketing.DigitalMarketing.Services.DTOs.ArticleDtos
{
    public class ArticleDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Slug { get; set; } = null!;
        public string Summary { get; set; } = null!;
        public string Content { get; set; } = null!;
        public string? CoverImageUrl { get; set; }
        public DateTime PublishedAt { get; set; }
        public bool IsPublished { get; set; }

        public int ArticleCategoryId { get; set; }
        public string ArticleCategoryName { get; set; } = null!;
    }
}
