using Eneru.Services;
using Microsoft.AspNetCore.Mvc;

namespace Eneru.Controllers
{
    public class OrdersController : Controller
    {
        private readonly IOrderService _orders;
        private readonly ICartService _cart;

        public OrdersController(IOrderService orders, ICartService cart)
        {
            _orders = orders;
            _cart = cart;
        }

        [HttpGet]
        public async Task<IActionResult> Checkout()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) return RedirectToAction("Login", "Account");

            var cartItems = await _cart.GetCartAsync(userId.Value);
            if (!cartItems.Any()) return RedirectToAction("Index", "Cart");

            ViewBag.Total = cartItems.Sum(c => c.Product!.Price * c.Quantity);
            return View(cartItems);
        }

        [HttpPost]
        public async Task<IActionResult> PlaceOrder()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) return RedirectToAction("Login", "Account");

            var cartItems = await _cart.GetCartAsync(userId.Value);
            if (!cartItems.Any()) return RedirectToAction("Index", "Cart");

            var order = await _orders.PlaceOrderAsync(userId.Value, cartItems);
            HttpContext.Session.SetInt32("CartCount", 0);

            return RedirectToAction("Confirmation", new { id = order.Id });
        }

        [HttpGet]
        public async Task<IActionResult> Confirmation(int id)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) return RedirectToAction("Login", "Account");

            var order = await _orders.GetOrderAsync(id, userId.Value);
            if (order == null) return NotFound();
            return View(order);
        }

        [HttpGet]
        public async Task<IActionResult> MyOrders()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) return RedirectToAction("Login", "Account");

            var orders = await _orders.GetUserOrdersAsync(userId.Value);
            return View(orders);
        }
    }
}