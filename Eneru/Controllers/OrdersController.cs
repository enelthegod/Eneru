using Eneru.Data;
using Eneru.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Eneru.Controllers
{
    public class OrdersController : Controller
    {
        private readonly AppDbContext _db;

        public OrdersController(AppDbContext db)
        {
            _db = db;
        }

        // GET /Orders/Checkout
        public async Task<IActionResult> Checkout()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
                return RedirectToAction("Login", "Account");

            // Load cart items with product details
            var cartItems = await _db.CartItems
                .Include(c => c.Product)
                .Where(c => c.UserId == userId)
                .ToListAsync();

            // Redirect back to cart if empty
            if (!cartItems.Any())
                return RedirectToAction("Index", "Cart");

            ViewBag.Total = cartItems.Sum(c => c.Product!.Price * c.Quantity);
            return View(cartItems);
        }

        // POST /Orders/PlaceOrder
        [HttpPost]
        public async Task<IActionResult> PlaceOrder()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
                return RedirectToAction("Login", "Account");

            // Load cart items
            var cartItems = await _db.CartItems
                .Include(c => c.Product)
                .Where(c => c.UserId == userId)
                .ToListAsync();

            if (!cartItems.Any())
                return RedirectToAction("Index", "Cart");

            // Create the order
            var order = new Order
            {
                UserId = userId.Value,
                CreatedAt = DateTime.UtcNow,
                Status = OrderStatus.Pending,
                TotalPrice = cartItems.Sum(c => c.Product!.Price * c.Quantity)
            };

            _db.Orders.Add(order);
            await _db.SaveChangesAsync(); // Save to get the generated order Id

            // Create order items from cart — store price at time of purchase
            foreach (var cartItem in cartItems)
            {
                _db.OrderItems.Add(new OrderItem
                {
                    OrderId = order.Id,
                    ProductId = cartItem.ProductId,
                    Quantity = cartItem.Quantity,
                    PriceAtPurchase = cartItem.Product!.Price
                });
            }

            // Clear the cart after placing order
            _db.CartItems.RemoveRange(cartItems);

            await _db.SaveChangesAsync();

            // Redirect to confirmation page with the new order id
            return RedirectToAction("Confirmation", new { id = order.Id });
        }

        // GET /Orders/Confirmation/5
        public async Task<IActionResult> Confirmation(int id)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
                return RedirectToAction("Login", "Account");

            var order = await _db.Orders
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                .FirstOrDefaultAsync(o => o.Id == id && o.UserId == userId);

            if (order == null)
                return NotFound();

            return View(order);
        }

        // GET /Orders/MyOrders
        public async Task<IActionResult> MyOrders()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
                return RedirectToAction("Login", "Account");

            // Load all orders for this user, newest first
            var orders = await _db.Orders
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                .Where(o => o.UserId == userId)
                .OrderByDescending(o => o.CreatedAt)
                .ToListAsync();

            return View(orders);
        }
    }
}