using AutoMapper;
using DigitalMarketing.DigitalMarketing.Services.DTOs.ArticleDtos;
using DigitalMarketing.DigitalMarketing.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DigitalMarketing.Admin.Controllers
{
    public class ArticlesController : Controller
    {
        private readonly IArticleService _articleService;
        private readonly IArticleCategoryService _categoryService;
        private readonly IMapper _mapper;
        public ArticlesController(IArticleService articleService, IArticleCategoryService categoryService,
            IMapper mapper)
        {
            _articleService = articleService;
            _categoryService = categoryService;
            _mapper = mapper;
        }





        public async Task<IActionResult> Index()
        {
            var articles = await _articleService.GetAllAsync();

            return View(articles);
        }



        [HttpGet]
        public async Task<IActionResult> Create()
        {
            await LoadCategoriesAsync();
            return View(new CreateArticleDto());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateArticleDto dto, IFormFile? coverImage)
        {

            var result = await _articleService.CreateAsync(dto);
            if (!result.Success)
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError(string.Empty, error);

                await LoadCategoriesAsync();
                return View(dto);
            }



            TempData["Success"] = "مقاله با موفقیت ثبت شد.";
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
            var article = await _articleService.GetByIdAsync(id);
            if (article == null)
                return NotFound();

            await LoadCategoriesAsync();


            var dtoo = _mapper.Map<UpdateArticleDto>(article);


            ViewBag.CurrentCoverImage = article.CoverImageUrl;
            return View(dtoo);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Edit(int id, UpdateArticleDto dto, IFormFile? coverImage)
        {
            if (id != dto.Id)
                return BadRequest();


            var result = await _articleService.UpdateAsync(dto);
            if (!result.Success)
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError(string.Empty, error);

                await LoadCategoriesAsync();
                return View(dto);
            }



            TempData["Success"] = "مقاله ویرایش شد";
            return RedirectToAction(nameof(Index));
        }










        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var result= await _articleService.DeleteAsync(id);
            TempData[result.Success ? "Success" : "Error"] =
                result.Success ? "مقاله حذف شد." : string.Join(" ", result.Errors);

            return RedirectToAction(nameof(Index));
        }





        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> TogglePublish(int id)
        {
            var result = await _articleService.TogglePublishAsync(id);
            TempData[result.Success ? "Success" : "Error"] = 
                result.Success ? "وضعیت انتشار تغییر کرد" : string.Join(" ",result.Errors);

            return RedirectToAction(nameof(Index));
        }












    }
}
