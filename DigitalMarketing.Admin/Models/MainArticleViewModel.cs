namespace DigitalMarketing.Admin.Models
{
    public class MainArticleViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; } = null!;

        public string CategoryName { get; set; } = null!;

        public bool IsPublished { get; set; }

        public DateTime CreateDate { get; set; }
    }
}
