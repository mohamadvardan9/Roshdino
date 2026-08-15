using DigitalMarketing.Services.DigitalMarketing.Services.DTOs.ContactMessageDtos;
using DigitalMarketing.Services.DigitalMarketing.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DigitalMarketing.Web.Controllers
{
    public class ContactController : Controller
    {
        private readonly IContactMessageService _contactMessageService;
        public ContactController(IContactMessageService contactMessageService)
        {
            _contactMessageService = contactMessageService;
        }




        [Route("contact")]
        public IActionResult Index()
        {
            return View(new CreateContactMessageDto());
        }

        [HttpPost]
        [Route("contact")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(CreateContactMessageDto dto)
        {
            var result = await _contactMessageService.CreateAsync(dto);

            if(!result.Success)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error);
                    return View(dto);
                }
            }


            TempData["Success"] = "پیام شما با موفقیت ارسال شد. به‌زودی با شما تماس می‌گیریم.";
            return RedirectToAction(nameof(Index));
        }
    }
}
