using Microsoft.AspNetCore.Mvc;
using TradingCompany_WEB.Models;

namespace TradingCompany_WEB.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAuthService _authService; // Використовуємо IAuthService для автентифікації

        public AccountController(IAuthService authService)
        {
            _authService = authService ?? throw new ArgumentNullException(nameof(authService));
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            // Виклик методу AuthenticateAsync для автентифікації користувача
            var user = await _authService.AuthenticateAsync(model.UserName, model.Password);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Неправильне ім'я користувача або пароль.");
                return View(model);
            }

            // Збереження даних користувача в сесії
            HttpContext.Session.SetString("UserName", user.UserName);
            HttpContext.Session.SetString("UserRole", user.RoleName);

            return RedirectToAction("Index", "Product"); // Перенаправлення на головну сторінку продуктів
        }

        [HttpPost]
        public IActionResult Logout()
        {
            // Очищення сесії та перенаправлення на сторінку входу
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
