using AutoMapper;
using DigitalMarketing.DigitalMarketing.Services.DTOs.ProductDtos;
using DigitalMarketing.DigitalMarketing.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DigitalMarketing.Admin.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly IProductCategoryService _categoryService;
        private readonly IMapper _mapper;

        public ProductController(IProductService productService,IProductCategoryService categoryService,
            IMapper mapper)
        {
            _productService = productService;
            _categoryService = categoryService;
            _mapper = mapper;
        }



        public async Task<IActionResult> Index()
        {
            var result = await _productService.GetAllAsync();
            return View(result);
        }




        [HttpGet]
        public async Task <IActionResult> Create()
        {
            await LoadCategoriesAsync();
            return View(new CreateProductDto());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateProductDto dto)
        {
            var result = await _productService.CreateAsync(dto);

            if (!result.Success)
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError(string.Empty, error);

                await LoadCategoriesAsync();
                return View(dto);
            }

            TempData["Success"] = "محصول با موفقیت ثبت شد.";
            return RedirectToAction(nameof(Index));
        }

        private async Task LoadCategoriesAsync()
        {
            var categories = await _categoryService.GetAllAsync();
            ViewBag.Categories = categories;
        }






        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var product = await _productService.GetByIdAsync(id);
            if (product == null)
                return NotFound();

            await LoadCategoriesAsync();

            var dto = _mapper.Map<UpdateProductDto>(product);


            ViewBag.ExistingImages = product.Images; // برای نمایش تو View و مدیریت حذف/Main
            return View(dto);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UpdateProductDto dto)
        {
            if (id != dto.Id)
                return BadRequest();

            var result = await _productService.UpdateAsync(dto);

            if (!result.Success)
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError(string.Empty, error);

                await LoadCategoriesAsync();
                return View(dto);
            }

            TempData["Success"] = "محصول ویرایش شد.";
            return RedirectToAction(nameof(Index));
        }










        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _productService.DeleteAsync(id);
            TempData[result.Success ? "Success" : "Error"] =
                result.Success ? "محصول حذف شد." : string.Join(" ", result.Errors);

            return RedirectToAction(nameof(Index));
        }






        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveImage(int imageId, int productId)
        {

            var result = await _productService.RemoveImageAsync(imageId, productId);

            TempData[result.Success ? "Success" : "Error"] =
                result.Success ? "تصویر حذف شد." : string.Join(" ", result.Errors);

            return RedirectToAction(nameof(Edit), new { id = productId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SetMainImage(int imageId, int productId)
        {
            var result = await _productService.SetMainImageAsync(productId, imageId);

            TempData[result.Success ? "Success" : "Error"] =
                result.Success ? "تصویر اصلی تغییر کرد." : string.Join(" ", result.Errors);

            return RedirectToAction(nameof(Edit), new { id = productId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> TogglePublish(int id)
        {
            var result = await _productService.TogglePublishAsync(id);

            TempData[result.Success ? "Success" : "Error"] =
                result.Success ? "وضعیت انتشار تغییر کرد." : string.Join(" ", result.Errors);

            return RedirectToAction(nameof(Index));
        }





    }
}
