using DigitalMarketing.DigitalMarketing.Services.DTOs.ArticleCategoryDtos;
using DigitalMarketing.DigitalMarketing.Services.DTOs.ArticleDtos;

namespace DigitalMarketing.Web.Models
{
    public class ArticleListViewModel
    {
        public List<ArticleDto> Articles { get; set; } = new();
        public List<ArticleCategoryDto> Categories { get; set; } = new();
        public int? SelectedCategoryId { get; set; }
    }
}
