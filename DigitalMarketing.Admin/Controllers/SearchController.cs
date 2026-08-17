using DigitalMarketing.Services.DigitalMarketing.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DigitalMarketing.Admin.Controllers
{
    public class SearchController : Controller
    {
        private readonly IAdminSearchService _searchService;
        public SearchController(IAdminSearchService searchService)
        {
            _searchService = searchService;
        }


        public async Task<IActionResult> Index(string query)
        {
            var result = await _searchService.SearchAsync(query);

            return Json(result);
        }
    }
}
