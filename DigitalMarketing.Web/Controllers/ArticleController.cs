using DigitalMarketing.DigitalMarketing.Services.Interfaces;
using DigitalMarketing.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace DigitalMarketing.Web.Controllers
{
    public class ArticleController : Controller
    {
        private readonly IArticleService _articleService;
        private readonly IArticleCategoryService _articleCategoryService;
        public ArticleController(IArticleService articleService, IArticleCategoryService articleCategoryService)
        {
            _articleService = articleService;
            _articleCategoryService = articleCategoryService;
        }



        // GET: /articles
        // GET: /articles?categoryId=2
        [Route("articles")]
        public async Task<IActionResult> Index(int? categoryId)
        {
            // باید خوانده شود
            var articles = categoryId.HasValue
                ? await _articleService.GetByCategoryAsync(categoryId.Value)
                : await _articleService.GetPublishedAsync();

            var categories = await _articleCategoryService.GetAllAsync();

            var viewModel = new ArticleListViewModel
            {
                Articles = articles,
                Categories = categories,
                SelectedCategoryId = categoryId
            };

            return View(viewModel);
        }






        // GET: /articles/{slug}
        [Route("articles/{slug}")]
        public async Task <IActionResult> Details(string slug)
        {
            var article = await _articleService.GetBySlugAsync(slug);

            if (article == null) return NotFound();

            return View(article);
        }
    }
}
