using DigitalMarketing.DigitalMarketing.Services.DTOs.ProductCategoryDtos;
using DigitalMarketing.DigitalMarketing.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DigitalMarketing.Admin.Controllers
{

    public class ProductCategoriesController : Controller
    {
        private readonly IProductCategoryService _service;
        public ProductCategoriesController(IProductCategoryService service)
        {
            _service = service;
        }







        // GET : /ProductCategories
        public async Task<IActionResult> Index()
        {
            var categories = await _service.GetAllAsync();
            return View(categories);
        }



        // GET : /ProductCategories/Create
        public IActionResult Create()
        {
            return View(new CreateProductCategoryDto());
        }

        // POST /ProductCategories/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateProductCategoryDto model)
        {
            if(!ModelState.IsValid)
                return View(model);


            var result = await _service.CreateAsync(model);
            if (!result.Success)
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError(string.Empty, error);


                return View(model);
            }


            TempData["Success"] = "دسته‌بندی با موفقیت ثبت شد.";
            return RedirectToAction(nameof(Index));
        }




        // GET : /ProductCategories/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var category = await _service.GetByIdAsync(id);
            if (category == null) return NotFound();

            var model = new UpdateProductCategoryDto
            {
                Id = category.Id,
                Name = category.Name
            };

            return View(model);
        }

        // POST : /ProductCategories/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UpdateProductCategoryDto model)
        {
            if (!ModelState.IsValid)
                return View(model);


            var result = await _service.UpdateAsync(id , model);
            if (!result.Success)
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError(string.Empty, error);

                return View(model);
            }


            TempData["Success"] = "دسته‌بندی با موفقیت ویرایش شد.";
            return RedirectToAction(nameof(Index));
        }





        // POST : /ProductCategories/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _service.DeleteAsync(id);
            if(!result.Success)
                TempData["Error"] = string.Join(" ", result.Errors);
            else
                TempData["Success"] = "دسته‌بندی حذف شد.";


            return RedirectToAction(nameof(Index));
        }


















    }
}
