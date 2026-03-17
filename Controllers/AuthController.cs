using Microsoft.AspNetCore.Mvc;
using ClinicQueue.Services;
using ClinicQueue.ViewModels;
using ClinicQueue.Helpers;

namespace ClinicQueue.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            // If already logged in, redirect to dashboard
            var token = HttpContext.Session.GetString(SessionKeys.Token);
            if (!string.IsNullOrEmpty(token))
                return RedirectToAction("Index", "Home");

            return View(new LoginViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = await _authService.LoginAsync(model.Email, model.Password);

            if (result.Success && result.Data != null)
            {
                // Store JWT and user info in session
                HttpContext.Session.SetString(SessionKeys.Token, result.Data.Token);
                HttpContext.Session.SetObjectAsJson(SessionKeys.User, result.Data.User!);
                HttpContext.Session.SetString(SessionKeys.UserRole, result.Data.User?.Role ?? "");
                HttpContext.Session.SetString(SessionKeys.UserName, result.Data.User?.Name ?? "");
                HttpContext.Session.SetString(SessionKeys.ClinicName, result.Data.User?.ClinicName ?? "");

                // Redirect to role-specific dashboard
                return RedirectToAction("Index", "Home");
            }

            model.ErrorMessage = result.Error ?? "Invalid email or password";
            return View(model);
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
