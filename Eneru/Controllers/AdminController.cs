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
        private readonly ImageUploadService _imageUpload;

        // ASP.NET automatically injects both services here
        public AdminController(AppDbContext db, ImageUploadService imageUpload)
        {
            _db = db;
            _imageUpload = imageUpload;
        }

        // ─────────────────────────────────────────
        // DASHBOARD — GET /Admin
        // ─────────────────────────────────────────
        public async Task<IActionResult> Index()
        {
            if (!AdminGuard.IsAdmin(HttpContext.Session))
                return RedirectToAction("Index", "Home");

            ViewBag.ProductCount = await _db.Products.CountAsync();
            ViewBag.OrderCount = await _db.Orders.CountAsync();
            ViewBag.UserCount = await _db.Users.CountAsync();

            return View();
        }

        // ─────────────────────────────────────────
        // PRODUCTS LIST — GET /Admin/Products
        // ─────────────────────────────────────────
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

        // ─────────────────────────────────────────
        // CREATE PRODUCT — GET /Admin/CreateProduct
        // Shows the empty form
        // ─────────────────────────────────────────
        public async Task<IActionResult> CreateProduct()
        {
            if (!AdminGuard.IsAdmin(HttpContext.Session))
                return RedirectToAction("Index", "Home");

            ViewBag.Categories = await _db.Categories.ToListAsync();
            return View();
        }

        // ─────────────────────────────────────────
        // CREATE PRODUCT — POST /Admin/CreateProduct
        // Handles form submission with optional image upload
        // ─────────────────────────────────────────
        [HttpPost]
        public async Task<IActionResult> CreateProduct(
            string name, string description, decimal price,
            string brand, int categoryId,
            IFormFile? imageFile,   // uploaded file from the form
            string? imageUrl)       // fallback: manual URL if no file uploaded
        {
            if (!AdminGuard.IsAdmin(HttpContext.Session))
                return RedirectToAction("Index", "Home");

            // Try to save uploaded image first
            // If no file uploaded — fall back to manual URL
            // If no URL either — use placeholder
            var savedImageUrl = await _imageUpload.SaveImageAsync(imageFile)
                ?? imageUrl
                ?? "/images/placeholder.jpg";

            _db.Products.Add(new Product
            {
                Name = name,
                Description = description,
                Price = price,
                Brand = brand,
                CategoryId = categoryId,
                ImageUrl = savedImageUrl,
                IsAvailable = true,
                CreatedAt = DateTime.UtcNow
            });

            await _db.SaveChangesAsync();
            return RedirectToAction("Products");
        }

        // ─────────────────────────────────────────
        // EDIT PRODUCT — GET /Admin/EditProduct/5
        // Loads existing product into form
        // ─────────────────────────────────────────
        public async Task<IActionResult> EditProduct(int id)
        {
            if (!AdminGuard.IsAdmin(HttpContext.Session))
                return RedirectToAction("Index", "Home");

            var product = await _db.Products.FindAsync(id);
            if (product == null) return NotFound();

            ViewBag.Categories = await _db.Categories.ToListAsync();
            return View(product);
        }

        // ─────────────────────────────────────────
        // EDIT PRODUCT — POST /Admin/EditProduct
        // Saves changes with optional new image
        // ─────────────────────────────────────────
        [HttpPost]
        public async Task<IActionResult> EditProduct(
            int id, string name, string description, decimal price,
            string brand, int categoryId,
            IFormFile? imageFile,
            string? imageUrl,
            bool isAvailable)
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
            product.IsAvailable = isAvailable;

            // Only update image if a new file was uploaded or new URL provided
            var newImageUrl = await _imageUpload.SaveImageAsync(imageFile) ?? imageUrl;
            if (!string.IsNullOrEmpty(newImageUrl))
                product.ImageUrl = newImageUrl;

            await _db.SaveChangesAsync();
            return RedirectToAction("Products");
        }

        // ─────────────────────────────────────────
        // DELETE PRODUCT — POST /Admin/DeleteProduct
        // ─────────────────────────────────────────
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

        // ─────────────────────────────────────────
        // ORDERS LIST — GET /Admin/Orders
        // ─────────────────────────────────────────
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

        // ─────────────────────────────────────────
        // UPDATE ORDER STATUS — POST /Admin/UpdateOrderStatus
        // ─────────────────────────────────────────
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