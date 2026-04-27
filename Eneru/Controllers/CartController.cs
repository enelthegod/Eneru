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

        // GET /Cart
        public async Task<IActionResult> Index()
        {
            // Redirect to login if not authenticated
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
                return RedirectToAction("Login", "Account");

            // Get all cart items for this user, including product details
            var items = await _db.CartItems
                .Include(c => c.Product)
                .Where(c => c.UserId == userId)
                .ToListAsync();

            // Calculate total price
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

            // Check if this product is already in the cart
            var existing = await _db.CartItems
                .FirstOrDefaultAsync(c => c.UserId == userId && c.ProductId == productId);

            if (existing != null)
            {
                // If already in cart — just increase quantity
                existing.Quantity += quantity;
            }
            else
            {
                // Otherwise add new cart item
                _db.CartItems.Add(new CartItem
                {
                    UserId = userId.Value,
                    ProductId = productId,
                    Quantity = quantity
                });
            }

            await _db.SaveChangesAsync();
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
                // If quantity is 0 or less — remove item completely
                if (quantity <= 0)
                    _db.CartItems.Remove(item);
                else
                    item.Quantity = quantity;

                await _db.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }
    }
}