using Newtonsoft.Json;

namespace ClinicQueue.Models
{
    public class Appointment
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("appointmentDate")]
        public string? AppointmentDate { get; set; }

        [JsonProperty("timeSlot")]
        public string? TimeSlot { get; set; }

        [JsonProperty("status")]
        public string? Status { get; set; }

        [JsonProperty("patientId")]
        public int PatientId { get; set; }

        [JsonProperty("clinicId")]
        public int ClinicId { get; set; }

        [JsonProperty("createdAt")]
        public DateTime? CreatedAt { get; set; }

        [JsonProperty("queueEntry")]
        public QueueEntry? QueueEntry { get; set; }

        [JsonProperty("prescription")]
        public Prescription? Prescription { get; set; }

        [JsonProperty("report")]
        public Report? Report { get; set; }
    }

    public class BookAppointmentRequest
    {
        [JsonProperty("appointmentDate")]
        public string AppointmentDate { get; set; } = string.Empty;

        [JsonProperty("timeSlot")]
        public string TimeSlot { get; set; } = string.Empty;
    }
}
