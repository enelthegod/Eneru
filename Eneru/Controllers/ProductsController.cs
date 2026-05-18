using Eneru.Data;
using Eneru.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Eneru.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductService _products;
        private readonly AppDbContext _db;

        public ProductsController(IProductService products, AppDbContext db)
        {
            _products = products;
            _db = db;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string? category, string? search)
        {
            var products = await _products.GetCatalogAsync(category, search);
            var categories = await _db.Categories.ToListAsync();

            ViewBag.CurrentCategory = category;
            ViewBag.CurrentSearch = search;
            ViewBag.Categories = categories;

            return View(products);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var product = await _products.GetDetailAsync(id);
            if (product == null) return NotFound();
            return View(product);
        }
    }
}