using Eneru.Services;
using Microsoft.AspNetCore.Mvc;

namespace Eneru.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartService _cart;

        public CartController(ICartService cart)
        {
            _cart = cart;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) return RedirectToAction("Login", "Account");

            var items = await _cart.GetCartAsync(userId.Value);
            ViewBag.Total = items.Sum(c => c.Product!.Price * c.Quantity);
            return View(items);
        }

        [HttpPost]
        public async Task<IActionResult> Add(int productId, int quantity = 1)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) return RedirectToAction("Login", "Account");

            await _cart.AddToCartAsync(userId.Value, productId, quantity);
            await RefreshCartCount(userId.Value);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Remove(int cartItemId)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) return RedirectToAction("Login", "Account");

            await _cart.RemoveFromCartAsync(cartItemId, userId.Value);
            await RefreshCartCount(userId.Value);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateQuantity(int cartItemId, int quantity)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) return RedirectToAction("Login", "Account");

            await _cart.UpdateQuantityAsync(cartItemId, userId.Value, quantity);
            await RefreshCartCount(userId.Value);
            return RedirectToAction("Index");
        }

        private async Task RefreshCartCount(int userId)
        {
            var count = await _cart.GetCartCountAsync(userId);
            HttpContext.Session.SetInt32("CartCount", count);
        }
    }
}