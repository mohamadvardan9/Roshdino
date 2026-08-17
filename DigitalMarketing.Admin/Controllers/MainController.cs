using AspNetCoreGeneratedDocument;
using DigitalMarketing.Admin.Models;
using DigitalMarketing.DigitalMarketing.Data;
using DigitalMarketing.Services.DigitalMarketing.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens.Experimental;

namespace DigitalMarketing.Admin.Controllers
{
    public class MainController : Controller
    {
        private readonly IMainService _service;
        public MainController(IMainService service)
        {
            _service = service;
        }




        public async Task<IActionResult> Index()
        {
            var stats = await _service.GetStatsAsync();

            var viewModel = new MainViewModel
            {
                ArticlesCount = stats.ArticlesCount,  
                ProductsCount = stats.ProductsCount,  

                DraftArticlesCount = stats.DraftArticlesCount,  
                DraftProductsCount = stats.DraftProductsCount,  

                UnreadMessagesCount = stats.UnreadMessagesCount,  

                ArticlesGrowthPercent = stats.ArticlesGrowthPercent,  
                ProductsGrowthPercent = stats.ProductsGrowthPercent,  

                LatestArticles = stats.LatestArticles,  
                LatestProducts = stats.LatestProducts   
            };

            return View(viewModel);
        }
    }
}
