using DigitalMarketing.Admin.Helpers;
using DigitalMarketing.DigitalMarketing.Services.DTOs.ProductDtos;
using DigitalMarketing.DigitalMarketing.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace DigitalMarketing.Admin.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly IProductCategoryService _categoryService;
        private readonly FileUploadHelper _fileUpload;

        public ProductController(IProductService productService,IProductCategoryService categoryService,
            FileUploadHelper fileUpload)
        {
            _productService = productService;
            _categoryService = categoryService;
            _fileUpload = fileUpload;
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
        public async Task<IActionResult> Create(CreateProductDto model, List<IFormFile> images)
        {
            foreach (var file in images.Where(f => f.Length > 0))
            {
                var (success, path, error) = await _fileUpload.SaveProductImageAsync(file);
                if (!success)
                {
                    ModelState.AddModelError(string.Empty, error!);
                    await LoadCategoriesAsync();
                    return View(model);
                }
                model.ImagePaths.Add(path!);
            }

            var result = await _productService.CreateAsync(model);

            if (!result.Success)
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError(string.Empty, error);

                await LoadCategoriesAsync();
                return View(model);
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

            var dto = new UpdateProductDto
            {
                Id = product.Id,
                Title = product.Title,
                ShortDescription = product.ShortDescription,
                Description = product.Description,
                Price = product.Price,
                ProductCategoryId = product.ProductCategoryId,
                IsPublished = product.IsPublished,
                Images = product.Images,
            };

            ViewBag.ExistingImages = product.Images; // برای نمایش تو View و مدیریت حذف/Main
            return View(dto);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UpdateProductDto dto, List<IFormFile> newImages)
        {
            if (id != dto.Id)
                return BadRequest();

            foreach (var file in newImages.Where(f => f.Length > 0))
            {
                var (success, path, error) = await _fileUpload.SaveProductImageAsync(file);
                if (!success)
                {
                    ModelState.AddModelError(string.Empty, error!);
                    await LoadCategoriesAsync();
                    return View(dto);
                }
                dto.NewImagePaths.Add(path!);
            }

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
            var result = await _productService.RemoveImageAsync(
                imageId, productId, _fileUpload.DeleteProductImage);

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
