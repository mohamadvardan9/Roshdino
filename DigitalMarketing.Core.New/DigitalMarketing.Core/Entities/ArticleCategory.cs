namespace DigitalMarketing.DigitalMarketing.Core.Entities
{
    public class ArticleCategory : BaseEntity
    {
        public string Name { get; set; } = null!;
        public string Slug { get; set; } = null!;

        public ICollection<Article> Articles { get; set; } = new List<Article>();
    }
}
