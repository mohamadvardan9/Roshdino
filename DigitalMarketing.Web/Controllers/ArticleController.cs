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
        [Route("articles")]
        public async Task<IActionResult> Index()
        {
            var articles = await _articleService.GetPublishedAsync();
            var categories = await _articleCategoryService.GetAllAsync();

            var viewModel = new ArticleListViewModel
            {
                Articles = articles,
                Categories = categories,
                SelectedCategorySlug = null
            };

            return View(viewModel);
        }




        // GET: / articles/category/{slug}
        [Route("articles/category/{slug}")]
        public async Task<IActionResult> Category(string slug)
        {
            var category = await _articleCategoryService.GetBySlugAsync(slug);

            if (category == null)
                return NotFound();

            var articles = await _articleService.GetByCategoryAsync(category.Id);
            var categories = await _articleCategoryService.GetAllAsync();

            var viewModel = new ArticleListViewModel
            {
                Articles = articles,
                Categories = categories,
                SelectedCategorySlug = slug
            };

            return View("Index", viewModel); // از همون View قبلی (Index) استفاده می‌کنیم
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
