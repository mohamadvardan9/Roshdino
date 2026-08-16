using AspNetCoreGeneratedDocument;
using DigitalMarketing.Admin.Models;
using DigitalMarketing.DigitalMarketing.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DigitalMarketing.Admin.Controllers
{
    public class MainController : Controller
    {
        private readonly MyDbContext _context;
        public MainController(MyDbContext context)
        {
            _context = context;
        }



        public IActionResult Index()
        {
            MainViewModel model = new MainViewModel
            {
                ArticlesCount = _context.Articles.Count(),
                ProductsCount = _context.Products.Count(),
                ArticlesGrowth = 12.5
            };


            model.LatestArticles = _context.Articles
                .OrderByDescending(a => a.CreatedAt)
                .Take(5)
                .Select(a => new MainArticleViewModel
                {
                    Id = a.Id,
                    Title = a.Title,
                    CategoryName = a.ArticleCategory.Name,
                    IsPublished = a.IsPublished,
                    CreateDate = a.CreatedAt
                })
                .ToList();

            return View(model);
        }
    }
}
