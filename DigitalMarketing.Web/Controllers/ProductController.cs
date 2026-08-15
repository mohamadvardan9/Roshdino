using DigitalMarketing.DigitalMarketing.Services.Interfaces;
using DigitalMarketing.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace DigitalMarketing.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly IProductCategoryService _productCategoryService;
        public ProductController(IProductService productService, IProductCategoryService productCategoryService)
        {
            _productService = productService;
            _productCategoryService = productCategoryService;
        }


        // GET: /products
        //GET: /products?categoryId=3
        [Route("products")]
        public async Task <IActionResult> Index(int? categoryId)
        {
            var products = categoryId.HasValue
                ? await _productService.GetByCategoryAsync(categoryId.Value)
                : await _productService.GetPublishedAsync();

            var categoroes = await _productCategoryService.GetAllAsync();

            var viewModel = new ProductListViewModel
            {
                Products = products,
                Categories = categoroes,
                SelectedCategoryId = categoryId
            };


            return View(viewModel);
        }



        // GET: /products/{slug}
        [Route("products/{slug}")]
        public async Task<IActionResult> Details(string slug)
        {
            var product = await _productService.GetBySlugAsync(slug);

            if (product == null) return NotFound();

            return View(product);
        }
    }
}
