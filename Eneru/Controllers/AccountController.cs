using Eneru.Services;
using Microsoft.AspNetCore.Mvc;

namespace Eneru.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService _account;
        private readonly ICartService _cart;

        public AccountController(IAccountService account, ICartService cart)
        {
            _account = account;
            _cart = cart;
        }

        [HttpGet]
        public IActionResult Register() => View();

        [HttpPost]
        public async Task<IActionResult> Register(string name, string email, string password)
        {
            if (await _account.EmailExistsAsync(email))
            {
                ViewBag.Error = "Email already registered.";
                return View();
            }

            var user = await _account.RegisterAsync(name, email, password);

            HttpContext.Session.SetInt32("UserId", user.Id);
            HttpContext.Session.SetString("UserName", user.Name);
            HttpContext.Session.SetString("UserEmail", user.Email);
            HttpContext.Session.SetInt32("CartCount", 0);

            return RedirectToAction("Index", "Products");
        }

        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            if (email == AdminGuard.AdminEmail && password == AdminGuard.AdminPassword)
            {
                HttpContext.Session.SetString("UserEmail", email);
                HttpContext.Session.SetString("UserName", "Admin");
                HttpContext.Session.SetInt32("UserId", 0);
                HttpContext.Session.SetInt32("CartCount", 0);
                return RedirectToAction("Index", "Admin");
            }

            var user = await _account.LoginAsync(email, password);
            if (user == null)
            {
                ViewBag.Error = "Invalid email or password.";
                return View();
            }

            HttpContext.Session.SetInt32("UserId", user.Id);
            HttpContext.Session.SetString("UserName", user.Name);
            HttpContext.Session.SetString("UserEmail", user.Email);

            var cartCount = await _cart.GetCartCountAsync(user.Id);
            HttpContext.Session.SetInt32("CartCount", cartCount);

            return RedirectToAction("Index", "Products");
        }

        [HttpPost]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}