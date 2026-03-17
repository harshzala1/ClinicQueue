using Microsoft.AspNetCore.Mvc;
using ClinicQueue.Services;
using ClinicQueue.ViewModels;
using ClinicQueue.Helpers;

namespace ClinicQueue.Controllers
{
    [AuthorizeRole("receptionist")]
    public class ReceptionistController : Controller
    {
        private readonly IQueueService _queueService;

        public ReceptionistController(IQueueService queueService)
        {
            _queueService = queueService;
        }

        public async Task<IActionResult> Dashboard(string? date)
        {
            var selectedDate = date ?? DateTime.Today.ToString("yyyy-MM-dd");
            var token = HttpContext.Session.GetString(SessionKeys.Token)!;

            var vm = new ReceptionistDashboardViewModel
            {
                SelectedDate = selectedDate
            };

            var result = await _queueService.GetQueueAsync(token, selectedDate);
            if (result.Success)
                vm.QueueEntries = result.Data ?? new();
            else
                TempData["Error"] = result.Error;

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateStatus(int id, string status, string? date)
        {
            var token = HttpContext.Session.GetString(SessionKeys.Token)!;
            var result = await _queueService.UpdateQueueStatusAsync(token, id, status);

            if (result.Success)
                TempData["Success"] = "Queue status updated successfully!";
            else
                TempData["Error"] = result.Error ?? "Failed to update status.";

            return RedirectToAction("Dashboard", new { date });
        }
    }
}
