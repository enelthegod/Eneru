using Eneru.Data;
using Eneru.Models;
using Eneru.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Eneru.Controllers
{
    public class AdminController : Controller
    {
        private readonly AppDbContext _db;

        public AdminController(AppDbContext db)
        {
            _db = db;
        }

        // GET /Admin
        public async Task<IActionResult> Index()
        {
            if (!AdminGuard.IsAdmin(HttpContext.Session))
                return RedirectToAction("Index", "Home");

            ViewBag.ProductCount = await _db.Products.CountAsync();
            ViewBag.OrderCount = await _db.Orders.CountAsync();
            ViewBag.UserCount = await _db.Users.CountAsync();

            return View();
        }

        // GET /Admin/Products
        public async Task<IActionResult> Products()
        {
            if (!AdminGuard.IsAdmin(HttpContext.Session))
                return RedirectToAction("Index", "Home");

            var products = await _db.Products
                .Include(p => p.Category)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();

            return View(products);
        }

        // GET /Admin/CreateProduct
        public async Task<IActionResult> CreateProduct()
        {
            if (!AdminGuard.IsAdmin(HttpContext.Session))
                return RedirectToAction("Index", "Home");

            ViewBag.Categories = await _db.Categories.ToListAsync();
            return View();
        }

        // POST /Admin/CreateProduct
        [HttpPost]
        public async Task<IActionResult> CreateProduct(
            string name, string description, decimal price,
            string brand, int categoryId, string imageUrl)
        {
            if (!AdminGuard.IsAdmin(HttpContext.Session))
                return RedirectToAction("Index", "Home");

            _db.Products.Add(new Product
            {
                Name = name,
                Description = description,
                Price = price,
                Brand = brand,
                CategoryId = categoryId,
                ImageUrl = string.IsNullOrEmpty(imageUrl) ? "/images/placeholder.jpg" : imageUrl,
                IsAvailable = true,
                CreatedAt = DateTime.UtcNow
            });

            await _db.SaveChangesAsync();
            return RedirectToAction("Products");
        }

        // GET /Admin/EditProduct/5
        public async Task<IActionResult> EditProduct(int id)
        {
            if (!AdminGuard.IsAdmin(HttpContext.Session))
                return RedirectToAction("Index", "Home");

            var product = await _db.Products.FindAsync(id);
            if (product == null) return NotFound();

            ViewBag.Categories = await _db.Categories.ToListAsync();
            return View(product);
        }

        // POST /Admin/EditProduct
        [HttpPost]
        public async Task<IActionResult> EditProduct(
            int id, string name, string description, decimal price,
            string brand, int categoryId, string imageUrl, bool isAvailable)
        {
            if (!AdminGuard.IsAdmin(HttpContext.Session))
                return RedirectToAction("Index", "Home");

            var product = await _db.Products.FindAsync(id);
            if (product == null) return NotFound();

            product.Name = name;
            product.Description = description;
            product.Price = price;
            product.Brand = brand;
            product.CategoryId = categoryId;
            product.ImageUrl = imageUrl;
            product.IsAvailable = isAvailable;

            await _db.SaveChangesAsync();
            return RedirectToAction("Products");
        }

        // POST /Admin/DeleteProduct
        [HttpPost]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            if (!AdminGuard.IsAdmin(HttpContext.Session))
                return RedirectToAction("Index", "Home");

            var product = await _db.Products.FindAsync(id);
            if (product != null)
            {
                _db.Products.Remove(product);
                await _db.SaveChangesAsync();
            }

            return RedirectToAction("Products");
        }

        // GET /Admin/Orders
        public async Task<IActionResult> Orders()
        {
            if (!AdminGuard.IsAdmin(HttpContext.Session))
                return RedirectToAction("Index", "Home");

            var orders = await _db.Orders
                .Include(o => o.User)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                .OrderByDescending(o => o.CreatedAt)
                .ToListAsync();

            return View(orders);
        }

        // POST /Admin/UpdateOrderStatus
        [HttpPost]
        public async Task<IActionResult> UpdateOrderStatus(int orderId, OrderStatus status)
        {
            if (!AdminGuard.IsAdmin(HttpContext.Session))
                return RedirectToAction("Index", "Home");

            var order = await _db.Orders.FindAsync(orderId);
            if (order != null)
            {
                order.Status = status;
                await _db.SaveChangesAsync();
            }

            return RedirectToAction("Orders");
        }
    }
}