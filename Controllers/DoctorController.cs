using Microsoft.AspNetCore.Mvc;
using ClinicQueue.Services;
using ClinicQueue.ViewModels;
using ClinicQueue.Models;
using ClinicQueue.Helpers;

namespace ClinicQueue.Controllers
{
    [AuthorizeRole("doctor")]
    public class DoctorController : Controller
    {
        private readonly IDoctorService _doctorService;

        public DoctorController(IDoctorService doctorService)
        {
            _doctorService = doctorService;
        }

        public async Task<IActionResult> Dashboard()
        {
            var token = HttpContext.Session.GetString(SessionKeys.Token)!;
            var vm = new DoctorDashboardViewModel();

            var result = await _doctorService.GetTodayQueueAsync(token);
            if (result.Success)
                vm.QueueItems = result.Data ?? new();
            else
                TempData["Error"] = result.Error;

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddPrescription(int appointmentId, string? patientName,
            string medicineNames, string medicineDosages, string medicineDurations, string? notes)
        {
            var token = HttpContext.Session.GetString(SessionKeys.Token)!;

            // Parse comma-separated medicine entries
            var names = (medicineNames ?? "").Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
            var dosages = (medicineDosages ?? "").Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
            var durations = (medicineDurations ?? "").Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

            var medicines = new List<PrescriptionMedicine>();
            for (int i = 0; i < names.Length; i++)
            {
                medicines.Add(new PrescriptionMedicine
                {
                    Name = names[i],
                    Dosage = i < dosages.Length ? dosages[i] : "",
                    Duration = i < durations.Length ? durations[i] : ""
                });
            }

            if (medicines.Count == 0)
            {
                TempData["Error"] = "Please add at least one medicine.";
                return RedirectToAction("Dashboard");
            }

            var request = new AddPrescriptionRequest
            {
                Medicines = medicines,
                Notes = notes
            };

            var result = await _doctorService.AddPrescriptionAsync(token, appointmentId, request);
            TempData[result.Success ? "Success" : "Error"] =
                result.Success ? "Prescription added successfully!" : (result.Error ?? "Failed to add prescription.");

            return RedirectToAction("Dashboard");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddReport(int appointmentId, string diagnosis,
            string? testRecommended, string? remarks)
        {
            var token = HttpContext.Session.GetString(SessionKeys.Token)!;

            if (string.IsNullOrWhiteSpace(diagnosis))
            {
                TempData["Error"] = "Diagnosis is required.";
                return RedirectToAction("Dashboard");
            }

            var request = new AddReportRequest
            {
                Diagnosis = diagnosis,
                TestRecommended = testRecommended,
                Remarks = remarks
            };

            var result = await _doctorService.AddReportAsync(token, appointmentId, request);
            TempData[result.Success ? "Success" : "Error"] =
                result.Success ? "Report added successfully!" : (result.Error ?? "Failed to add report.");

            return RedirectToAction("Dashboard");
        }
    }
}
