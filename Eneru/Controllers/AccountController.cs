using Eneru.Data;
using Eneru.Models;
using Eneru.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Eneru.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _db;

        public AccountController(AppDbContext db)
        {
            _db = db;
        }

        // GET /Account/Register
        public IActionResult Register() => View();

        // POST /Account/Register
        [HttpPost]
        public async Task<IActionResult> Register(string name, string email, string password)
        {
            // Check if email is already taken
            var exists = await _db.Users.AnyAsync(u => u.Email == email);
            if (exists)
            {
                ViewBag.Error = "Email already registered.";
                return View();
            }

            var user = new User
            {
                Name = name,
                Email = email,
                PasswordHash = PasswordHasher.Hash(password),
                CreatedAt = DateTime.UtcNow
            };

            _db.Users.Add(user);
            await _db.SaveChangesAsync();

            HttpContext.Session.SetInt32("UserId", user.Id);
            HttpContext.Session.SetString("UserName", user.Name);
            HttpContext.Session.SetString("UserEmail", user.Email);

            // New users have empty cart so set counter to 0
            HttpContext.Session.SetInt32("CartCount", 0);

            return RedirectToAction("Index", "Products");
        }

        // GET /Account/Login
        public IActionResult Login() => View();

        // POST /Account/Login
        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            // Check static admin credentials first — no database lookup needed
            if (email == AdminGuard.AdminEmail && password == AdminGuard.AdminPassword)
            {
                HttpContext.Session.SetString("UserEmail", email);
                HttpContext.Session.SetString("UserName", "Admin");
                HttpContext.Session.SetInt32("UserId", 0);
                HttpContext.Session.SetInt32("CartCount", 0);
                return RedirectToAction("Index", "Admin");
            }

            // Regular user login
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null || !PasswordHasher.Verify(password, user.PasswordHash))
            {
                ViewBag.Error = "Invalid email or password.";
                return View();
            }

            HttpContext.Session.SetInt32("UserId", user.Id);
            HttpContext.Session.SetString("UserName", user.Name);
            HttpContext.Session.SetString("UserEmail", user.Email);

            // Load existing cart count from database on login
            var cartCount = await _db.CartItems
                .Where(c => c.UserId == user.Id)
                .SumAsync(c => c.Quantity);
            HttpContext.Session.SetInt32("CartCount", cartCount);

            return RedirectToAction("Index", "Products");
        }

        // POST /Account/Logout
        [HttpPost]
        public IActionResult Logout()
        {
            // Clear all session data including cart count
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}