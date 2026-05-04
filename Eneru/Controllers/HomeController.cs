using Eneru.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Eneru.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _db;

        public HomeController(AppDbContext db)
        {
            _db = db;
        }

        // GET /
        public async Task<IActionResult> Index()
        {
            // Load 6 newest available products for the featured section
            var featuredProducts = await _db.Products
                .Include(p => p.Category)
                .Where(p => p.IsAvailable)
                .OrderByDescending(p => p.CreatedAt)
                .Take(6)
                .ToListAsync();

            return View(featuredProducts);
        }
    }
}