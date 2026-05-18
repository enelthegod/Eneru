using Eneru.Data;
using Eneru.Models;
using Eneru.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Eneru.Controllers
{
    public class AdminController : Controller
    {
        private readonly IProductService _products;
        private readonly IOrderService _orders;
        private readonly IAdminService _admin;
        private readonly IImageUploadService _imageUpload;
        private readonly AppDbContext _db;

        public AdminController(
            IProductService products,
            IOrderService orders,
            IAdminService admin,
            IImageUploadService imageUpload,
            AppDbContext db)
        {
            _products = products;
            _orders = orders;
            _admin = admin;
            _imageUpload = imageUpload;
            _db = db;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            if (!AdminGuard.IsAdmin(HttpContext.Session))
                return RedirectToAction("Index", "Home");

            var (products, orders, users) = await _admin.GetDashboardStatsAsync();
            ViewBag.ProductCount = products;
            ViewBag.OrderCount = orders;
            ViewBag.UserCount = users;

            return View();
        }

        [HttpGet]
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

        [HttpGet]
        public async Task<IActionResult> CreateProduct()
        {
            if (!AdminGuard.IsAdmin(HttpContext.Session))
                return RedirectToAction("Index", "Home");

            ViewBag.Categories = await _db.Categories.ToListAsync();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct(
            string name, string description, decimal price,
            string brand, int categoryId,
            IFormFile? imageFile, string? imageUrl)
        {
            if (!AdminGuard.IsAdmin(HttpContext.Session))
                return RedirectToAction("Index", "Home");

            var savedUrl = await _imageUpload.SaveImageAsync(imageFile)
                ?? imageUrl
                ?? "/images/placeholder.jpg";

            await _products.CreateAsync(
                name, description, price, brand, categoryId, savedUrl);

            return RedirectToAction("Products");
        }

        [HttpGet]
        public async Task<IActionResult> EditProduct(int id)
        {
            if (!AdminGuard.IsAdmin(HttpContext.Session))
                return RedirectToAction("Index", "Home");

            var product = await _products.GetDetailAsync(id);
            if (product == null) return NotFound();

            ViewBag.Categories = await _db.Categories.ToListAsync();
            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> EditProduct(
            int id, string name, string description, decimal price,
            string brand, int categoryId,
            IFormFile? imageFile, string? imageUrl, bool isAvailable)
        {
            if (!AdminGuard.IsAdmin(HttpContext.Session))
                return RedirectToAction("Index", "Home");

            var newImageUrl = await _imageUpload.SaveImageAsync(imageFile)
                ?? imageUrl
                ?? "";

            await _products.UpdateAsync(
                id, name, description, price,
                brand, categoryId, newImageUrl, isAvailable);

            return RedirectToAction("Products");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            if (!AdminGuard.IsAdmin(HttpContext.Session))
                return RedirectToAction("Index", "Home");

            await _products.DeleteAsync(id);
            return RedirectToAction("Products");
        }

        [HttpGet]
        public async Task<IActionResult> Orders()
        {
            if (!AdminGuard.IsAdmin(HttpContext.Session))
                return RedirectToAction("Index", "Home");

            var orders = await _orders.GetAllOrdersAsync();
            return View(orders);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateOrderStatus(int orderId, OrderStatus status)
        {
            if (!AdminGuard.IsAdmin(HttpContext.Session))
                return RedirectToAction("Index", "Home");

            await _orders.UpdateStatusAsync(orderId, status);
            return RedirectToAction("Orders");
        }
    }
}