using Microsoft.AspNetCore.Mvc;

namespace DigitalMarketing.Admin.Controllers
{
    public class MainController : Controller
    {

        public IActionResult Index()
        {
            return View();
        }
    }
}
