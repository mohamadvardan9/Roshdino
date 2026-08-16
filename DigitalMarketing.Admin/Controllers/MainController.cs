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


            return View(model);
        }
    }
}
