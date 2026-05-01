using Eneru.Data;
using Eneru.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Eneru.Controllers
{
    public class CartController : Controller
    {
        private readonly AppDbContext _db;

        public CartController(AppDbContext db)
        {
            _db = db;
        }

        // Recalculate total cart item count and save it in session
        private async Task RefreshCartCount(int userId)
        {
            var count = await _db.CartItems
                .Where(c => c.UserId == userId)
                .SumAsync(c => c.Quantity);

            HttpContext.Session.SetInt32("CartCount", count);
        }

        // GET /Cart
        public async Task<IActionResult> Index()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
                return RedirectToAction("Login", "Account");

            var items = await _db.CartItems
                .Include(c => c.Product)
                .Where(c => c.UserId == userId)
                .ToListAsync();

            ViewBag.Total = items.Sum(c => c.Product!.Price * c.Quantity);

            return View(items);
        }

        // POST /Cart/Add
        [HttpPost]
        public async Task<IActionResult> Add(int productId, int quantity = 1)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
                return RedirectToAction("Login", "Account");

            var existing = await _db.CartItems
                .FirstOrDefaultAsync(c => c.UserId == userId && c.ProductId == productId);

            if (existing != null)
            {
                existing.Quantity += quantity;
            }
            else
            {
                _db.CartItems.Add(new CartItem
                {
                    UserId = userId.Value,
                    ProductId = productId,
                    Quantity = quantity
                });
            }

            await _db.SaveChangesAsync();
            await RefreshCartCount(userId.Value);
            return RedirectToAction("Index");
        }

        // POST /Cart/Remove
        [HttpPost]
        public async Task<IActionResult> Remove(int cartItemId)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
                return RedirectToAction("Login", "Account");

            var item = await _db.CartItems
                .FirstOrDefaultAsync(c => c.Id == cartItemId && c.UserId == userId);

            if (item != null)
            {
                _db.CartItems.Remove(item);
                await _db.SaveChangesAsync();
                await RefreshCartCount(userId.Value);
            }

            return RedirectToAction("Index");
        }

        // POST /Cart/UpdateQuantity
        [HttpPost]
        public async Task<IActionResult> UpdateQuantity(int cartItemId, int quantity)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
                return RedirectToAction("Login", "Account");

            var item = await _db.CartItems
                .FirstOrDefaultAsync(c => c.Id == cartItemId && c.UserId == userId);

            if (item != null)
            {
                if (quantity <= 0)
                    _db.CartItems.Remove(item);
                else
                    item.Quantity = quantity;

                await _db.SaveChangesAsync();
                await RefreshCartCount(userId.Value);
            }

            return RedirectToAction("Index");
        }
    }
}