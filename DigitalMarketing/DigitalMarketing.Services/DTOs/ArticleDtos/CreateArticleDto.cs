namespace DigitalMarketing.DigitalMarketing.Services.DTOs.ArticleDtos
{
    public class CreateArticleDto
    {
        public string Title { get; set; } = null!;
        public string Summary { get; set; } = null!;
        public string Content { get; set; } = null!;
        public int ArticleCategoryId { get; set; }


        // Controller قبلش فایل رو ذخیره کرده و مسیرشو اینجا پاس میده
        public string? CoverImagePath { get; set; }
        public IFormFile? CoverImage { get; set; }
    }
}
