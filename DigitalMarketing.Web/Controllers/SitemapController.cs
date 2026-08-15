using DigitalMarketing.DigitalMarketing.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace DigitalMarketing.Web.Controllers
{
    public class SitemapController : Controller
    {
        private readonly IProductService _productService;
        private readonly IArticleService _articleService;
        public SitemapController(IProductService productService, IArticleService articleService)
        {
            _productService = productService;
            _articleService = articleService;
        }

        [Route("sitemap.xml")]
        public async Task<IActionResult> Index()
        {
            var baseUrl = $"{Request.Scheme}://{Request.Host}";
            var sb = new StringBuilder();

            sb.AppendLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
            sb.AppendLine("<urlset xmlns=\"http://www.sitemaps.org/schemas/sitemap/0.9\">");

            // صفحات ثابت
            AddUrl(sb, $"{baseUrl}/", "1.0");
            AddUrl(sb, $"{baseUrl}/products", "0.9");
            AddUrl(sb, $"{baseUrl}/articles", "0.9");
            AddUrl(sb, $"{baseUrl}/contact", "0.5");

            // محصولات
            var products = await _productService.GetPublishedAsync();
            foreach (var product in products)
                AddUrl(sb, $"{baseUrl}/products/{product.Slug}", "0.8");

            // مقالات
            var articles = await _articleService.GetPublishedAsync();
            foreach (var article in articles)
                AddUrl(sb, $"{baseUrl}/articles/{article.Slug}", "0.7");

            sb.AppendLine("</urlset>");

            return Content(sb.ToString(), "application/xml");
        }



        private void AddUrl(StringBuilder sb, string loc, string priority)
        {
            sb.AppendLine("<url>");
            sb.AppendLine($"<loc>{loc}</loc>");
            sb.AppendLine($"<priority>{priority}</priority>");
            sb.AppendLine("</url>");
        }


    }
}
