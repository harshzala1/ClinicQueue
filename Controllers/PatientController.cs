using Microsoft.AspNetCore.Mvc;
using ClinicQueue.Services;
using ClinicQueue.ViewModels;
using ClinicQueue.Models;
using ClinicQueue.Helpers;

namespace ClinicQueue.Controllers
{
    [AuthorizeRole("patient")]
    public class PatientController : Controller
    {
        private readonly IPatientService _patientService;

        public PatientController(IPatientService patientService)
        {
            _patientService = patientService;
        }

        public async Task<IActionResult> Dashboard()
        {
            var token = HttpContext.Session.GetString(SessionKeys.Token)!;
            var vm = new PatientDashboardViewModel();

            var apptResult = await _patientService.GetMyAppointmentsAsync(token);
            if (apptResult.Success)
                vm.Appointments = apptResult.Data ?? new();

            return View(vm);
        }

        public async Task<IActionResult> Appointments()
        {
            var token = HttpContext.Session.GetString(SessionKeys.Token)!;
            var result = await _patientService.GetMyAppointmentsAsync(token);
            return View(result.Success ? result.Data : new List<Appointment>());
        }

        public async Task<IActionResult> AppointmentDetails(int id)
        {
            var token = HttpContext.Session.GetString(SessionKeys.Token)!;
            var result = await _patientService.GetAppointmentDetailsAsync(token, id);
            if (!result.Success)
            {
                TempData["Error"] = result.Error;
                return RedirectToAction("Appointments");
            }
            return View(result.Data);
        }

        [HttpGet]
        public IActionResult BookAppointment()
        {
            return View(new BookAppointmentViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BookAppointment(BookAppointmentViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var token = HttpContext.Session.GetString(SessionKeys.Token)!;
            var request = new BookAppointmentRequest
            {
                AppointmentDate = model.AppointmentDate,
                TimeSlot = model.TimeSlot
            };

            var result = await _patientService.BookAppointmentAsync(token, request);
            if (result.Success)
            {
                TempData["Success"] = "Appointment booked successfully!";
                return RedirectToAction("Appointments");
            }

            TempData["Error"] = result.Error ?? "Failed to book appointment.";
            return View(model);
        }

        public async Task<IActionResult> Prescriptions()
        {
            var token = HttpContext.Session.GetString(SessionKeys.Token)!;
            var result = await _patientService.GetMyPrescriptionsAsync(token);
            return View(result.Success ? result.Data : new List<Prescription>());
        }

        public async Task<IActionResult> Reports()
        {
            var token = HttpContext.Session.GetString(SessionKeys.Token)!;
            var result = await _patientService.GetMyReportsAsync(token);
            return View(result.Success ? result.Data : new List<Report>());
        }
    }
}
