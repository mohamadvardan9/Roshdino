using DigitalMarketing.DigitalMarketing.Services.DTOs.ArticleCategoryDtos;
using DigitalMarketing.DigitalMarketing.Services.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace DigitalMarketing.Admin.Controllers
{
    public class ArticleCategoriesController : Controller
    {
        private readonly IArticleCategoryService _service;
        public ArticleCategoriesController(IArticleCategoryService service)
        {
            _service = service;
        }





        // GET: /ArticleCategories
        public async Task<IActionResult> Index()
        {
            var result = await _service.GetAllAsync();
            return View(result);
        }




        // GET: /ArticleCategories/Create
        public IActionResult Create()
        {
            return View(new CreateArticleCategoryDto());
        }

        // POST: /ArticleCategories/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateArticleCategoryDto model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var resut = await _service.CreateAsync(model);
            if(!resut.Success)
            {
                foreach (var error in resut.Errors)
                    ModelState.AddModelError(string.Empty, error);

                return View(model);
            }

            TempData["Success"] = "دسته‌بندی با موفقیت ثبت شد.";
            return RedirectToAction(nameof(Index));
        }






        // GET: /ArticleCategories/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var category = await _service.GetByIdAsync(id);
            if (category == null) return NotFound();

            var model = new UpdateArticleCategoryDto
            {
                Id = category.Id,
                Name = category.Name
            };

            return View(model);
        }

        // POST: /ArticleCategories/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UpdateArticleCategoryDto model)
        {
            if (!ModelState.IsValid)
                return View(model);


            var result = await _service.UpdateAsync(id,model);
            if (!result.Success)
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError(string.Empty, error);

                return View(model);
            }


            TempData["Success"] = "دسته‌بندی با موفقیت ویرایش شد.";
            return RedirectToAction(nameof(Index));
        }




        // POST: /ArticleCategories/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _service.DeleteAsync(id);
            if (!result.Success)
                TempData["Error"] = string.Join(" ", result.Errors);
            else
                TempData["Success"] = "دسته‌بندی حذف شد.";

            return RedirectToAction(nameof(Index));
        }



    }
}
