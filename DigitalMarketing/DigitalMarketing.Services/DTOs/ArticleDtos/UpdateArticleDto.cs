namespace DigitalMarketing.DigitalMarketing.Services.DTOs.ArticleDtos
{
    public class UpdateArticleDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Summary { get; set; } = null!;
        public string Content { get; set; } = null!;
        public int ArticleCategoryId { get; set; }
        public bool IsPublished { get; set; }



        // عکس جدیدی که کاربر انتخاب کرده
        public IFormFile? NewCoverImage { get; set; }

    }
}
