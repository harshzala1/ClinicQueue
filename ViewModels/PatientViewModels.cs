using ClinicQueue.Models;

namespace ClinicQueue.ViewModels
{
    public class PatientDashboardViewModel
    {
        public List<Appointment> Appointments { get; set; } = new();
        public List<Prescription> Prescriptions { get; set; } = new();
        public List<Report> Reports { get; set; } = new();
        public int TotalAppointments => Appointments.Count;
        public int TotalPrescriptions => Prescriptions.Count;
        public int TotalReports => Reports.Count;
    }

    public class BookAppointmentViewModel
    {
        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "Date is required")]
        public string AppointmentDate { get; set; } = string.Empty;

        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "Time slot is required")]
        public string TimeSlot { get; set; } = string.Empty;
    }
}
