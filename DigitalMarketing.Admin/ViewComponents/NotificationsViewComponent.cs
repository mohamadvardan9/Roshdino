using DigitalMarketing.Admin.Models;
using DigitalMarketing.Core.DigitalMarketing.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DigitalMarketing.Admin.ViewComponents
{
    public class NotificationsViewComponent : ViewComponent
    {
        private readonly IContactMessageRepository _repo;
        public NotificationsViewComponent(IContactMessageRepository repo)
        {
            _repo = repo;
        }


        public async Task<IViewComponentResult> InvokeAsync()
        {
            var unreadCount = await _repo.GetUnreadCountAsync();

            var model = new NotificationViewModel
            {
                UnreadCount = unreadCount
            };

            return View(model);
        }
    }
}
