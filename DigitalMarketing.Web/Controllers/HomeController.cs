using DigitalMarketing.DigitalMarketing.Services.Interfaces;
using DigitalMarketing.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace DigitalMarketing.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProductService _productService;
        private readonly IArticleService _articleService;

        public HomeController(IProductService productService, IArticleService articleService)
        {
            _productService = productService;
            _articleService = articleService;
        }

        public async Task<IActionResult> Index()
        {
            var products = await _productService.GetPublishedAsync();
            var articles = await _articleService.GetPublishedAsync();

            var viewModel = new HomeViewModel
            {
                ProductCount = products.Count,
                ArticleCount = articles.Count,
                FeaturedProducts = products.Take(6).ToList(),
                LatestArticles = articles.Take(3).ToList()
            };

            return View(viewModel);
        }
    }
}
