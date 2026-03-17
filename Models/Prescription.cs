using Newtonsoft.Json;

namespace ClinicQueue.Models
{
    public class Prescription
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("medicines")]
        public List<PrescriptionMedicine>? Medicines { get; set; }

        [JsonProperty("notes")]
        public string? Notes { get; set; }

        [JsonProperty("appointmentId")]
        public int AppointmentId { get; set; }

        [JsonProperty("doctor")]
        public object? Doctor { get; set; }

        [JsonProperty("appointment")]
        public object? Appointment { get; set; }

        [JsonProperty("createdAt")]
        public DateTime? CreatedAt { get; set; }
    }

    public class PrescriptionMedicine
    {
        [JsonProperty("name")]
        public string Name { get; set; } = string.Empty;

        [JsonProperty("dosage")]
        public string Dosage { get; set; } = string.Empty;

        [JsonProperty("duration")]
        public string Duration { get; set; } = string.Empty;
    }

    public class AddPrescriptionRequest
    {
        [JsonProperty("medicines")]
        public List<PrescriptionMedicine> Medicines { get; set; } = new();

        [JsonProperty("notes")]
        public string? Notes { get; set; }
    }
}
