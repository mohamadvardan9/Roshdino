using Microsoft.AspNetCore.Mvc;

namespace DigitalMarketing.Web.Controllers
{
    public class AboutUsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
