using ClinicQueue.Models;

namespace ClinicQueue.ViewModels
{
    public class DoctorDashboardViewModel
    {
        public List<DoctorQueueItem> QueueItems { get; set; } = new();
    }

    public class PrescriptionViewModel
    {
        public int AppointmentId { get; set; }
        public string? PatientName { get; set; }
        public List<MedicineEntry> Medicines { get; set; } = new()
        {
            new MedicineEntry()
        };
        public string? Notes { get; set; }
    }

    public class MedicineEntry
    {
        public string Name { get; set; } = string.Empty;
        public string Dosage { get; set; } = string.Empty;
        public string Duration { get; set; } = string.Empty;
    }

    public class ReportViewModel
    {
        public int AppointmentId { get; set; }
        public string? PatientName { get; set; }

        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "Diagnosis is required")]
        public string Diagnosis { get; set; } = string.Empty;
        public string? TestRecommended { get; set; }
        public string? Remarks { get; set; }
    }
}
