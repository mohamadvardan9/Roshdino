using DigitalMarketing.DigitalMarketing.Services.DTOs.ArticleDtos;
using DigitalMarketing.DigitalMarketing.Services.DTOs.ProductDtos;

namespace DigitalMarketing.Web.Models
{
    public class HomeViewModel
    {
        public int ProductCount { get; set; }
        public int ArticleCount { get; set; }

        public List<ProductDto> FeaturedProducts { get; set; } = new();
        public List<ArticleDto> LatestArticles { get; set; } = new();
    }
}
