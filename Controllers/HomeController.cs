using Microsoft.AspNetCore.Mvc;
using ClinicQueue.Helpers;

namespace ClinicQueue.Controllers
{
    /// <summary>
    /// Home controller — redirects to login or role-specific dashboard
    /// </summary>
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            var token = HttpContext.Session.GetString(SessionKeys.Token);
            if (string.IsNullOrEmpty(token))
                return RedirectToAction("Login", "Auth");

            var role = HttpContext.Session.GetString(SessionKeys.UserRole);
            return role?.ToLower() switch
            {
                "admin" => RedirectToAction("Dashboard", "Admin"),
                "doctor" => RedirectToAction("Dashboard", "Doctor"),
                "receptionist" => RedirectToAction("Dashboard", "Receptionist"),
                "patient" => RedirectToAction("Dashboard", "Patient"),
                _ => RedirectToAction("Login", "Auth")
            };
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View();
        }
    }
}
