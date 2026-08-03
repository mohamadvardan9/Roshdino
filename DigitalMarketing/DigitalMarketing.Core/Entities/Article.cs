namespace DigitalMarketing.DigitalMarketing.Core.Entities
{
    public class Article : BaseEntity
    {
        public string Title { get; set; } = null!;
        public string Slug { get; set; } = null!;
        public string Summary { get; set; } = null!;
        public string Content { get; set; } = null!;      // HTML از Rich Text Editor
        public string? CoverImageUrl { get; set; }
        public DateTime PublishedAt { get; set; } = DateTime.UtcNow;
        public bool IsPublished { get; set; } = true;

        public int ArticleCategoryId { get; set; }
        public ArticleCategory ArticleCategory { get; set; } = null!;
    }
}
