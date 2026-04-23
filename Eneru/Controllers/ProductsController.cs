using Eneru.Data;
using Eneru.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Eneru.Controllers
{
    public class ProductsController : Controller
    {
        private readonly AppDbContext _db;

        // ASP.NET automatically injects AppDbContext here (dependency injection)
        public ProductsController(AppDbContext db)
        {
            _db = db;
        }

        // GET /Products
        public async Task<IActionResult> Index(string? category, string? search)
        {
            // Start with all products, including their category data
            var query = _db.Products
                .Include(p => p.Category)
                .Where(p => p.IsAvailable)
                .AsQueryable();

            // Filter by category slug if provided (/Products?category=shoes)
            if (!string.IsNullOrEmpty(category))
            {
                query = query.Where(p => p.Category!.Slug == category);
            }

            // Filter by search term if provided (/Products?search=jeans)
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(p =>
                    p.Name.ToLower().Contains(search.ToLower()) ||
                    p.Brand.ToLower().Contains(search.ToLower()));
            }

            // Pass filter values back to the view so inputs stay filled
            ViewBag.CurrentCategory = category;
            ViewBag.CurrentSearch = search;

            // Pass all categories for the filter sidebar
            ViewBag.Categories = await _db.Categories.ToListAsync();

            var products = await query.ToListAsync();
            return View(products);
        }

        // GET /Products/Details/1
        public async Task<IActionResult> Details(int id)
        {
            var product = await _db.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.Id == id);

            // Return 404 if product doesn't exist
            if (product == null)
                return NotFound();

            return View(product);
        }
    }
}