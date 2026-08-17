using DigitalMarketing.Services.DigitalMarketing.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DigitalMarketing.Admin.Controllers
{
    public class ContactMessagesController : Controller
    {
        private readonly IContactMessageService _contactMessageService;
        public ContactMessagesController(IContactMessageService contactMessageService)
        {
            _contactMessageService = contactMessageService;
        }



        // GET: /ContactMessages/Index
        public async Task<IActionResult> Index()
        {
            var message = await _contactMessageService.GetAllAsync();
            return View(message);
        }


        // GET: /ContactMessages/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var message = await _contactMessageService.GetByIdAsync(id);
            if (message == null) return NotFound();


            // Automatically mark as read when opening details
            if (!message.IsRead)
                await _contactMessageService.MarkAsReadAsync(id);

            return View(message);
        }



        // POST: /ContactMessages/Delete/5
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _contactMessageService.DeleteAsync(id);

            TempData[result.Success ? "Success" : "Error"] =
                result.Success ? "پیام حذف شد." : string.Join(" ", result.Errors);

            return RedirectToAction(nameof(Index));
        }
    }
}
