using Microsoft.AspNetCore.Mvc;
using ClinicQueue.Services;
using ClinicQueue.ViewModels;
using ClinicQueue.Helpers;

namespace ClinicQueue.Controllers
{
    [AuthorizeRole("admin")]
    public class AdminController : Controller
    {
        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        public async Task<IActionResult> Dashboard()
        {
            var token = HttpContext.Session.GetString(SessionKeys.Token)!;
            var vm = new AdminDashboardViewModel();

            var clinicResult = await _adminService.GetClinicInfoAsync(token);
            if (clinicResult.Success)
                vm.Clinic = clinicResult.Data;

            var usersResult = await _adminService.GetUsersAsync(token);
            if (usersResult.Success)
                vm.Users = usersResult.Data ?? new();

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateUser(CreateUserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Please fill all required fields correctly.";
                return RedirectToAction("Dashboard");
            }

            var token = HttpContext.Session.GetString(SessionKeys.Token)!;
            var result = await _adminService.CreateUserAsync(token, new
            {
                name = model.Name,
                email = model.Email,
                password = model.Password,
                role = model.Role,
                phone = model.Phone
            });

            if (result.Success)
            {
                TempData["Success"] = $"User '{model.Name}' created successfully!";
            }
            else
            {
                TempData["Error"] = result.Error ?? "Failed to create user.";
            }

            return RedirectToAction("Dashboard");
        }
    }
}
