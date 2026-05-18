using Eneru.Services;
using Microsoft.AspNetCore.Mvc;

namespace Eneru.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProductService _products;

        public HomeController(IProductService products)
        {
            _products = products;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var featured = await _products.GetFeaturedAsync();
            return View(featured);
        }
    }
}