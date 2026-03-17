using Newtonsoft.Json;

namespace ClinicQueue.Models
{
    public class Report
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("diagnosis")]
        public string? Diagnosis { get; set; }

        [JsonProperty("testRecommended")]
        public string? TestRecommended { get; set; }

        [JsonProperty("remarks")]
        public string? Remarks { get; set; }

        [JsonProperty("appointmentId")]
        public int AppointmentId { get; set; }

        [JsonProperty("doctor")]
        public object? Doctor { get; set; }

        [JsonProperty("appointment")]
        public object? Appointment { get; set; }

        [JsonProperty("createdAt")]
        public DateTime? CreatedAt { get; set; }
    }

    public class AddReportRequest
    {
        [JsonProperty("diagnosis")]
        public string Diagnosis { get; set; } = string.Empty;

        [JsonProperty("testRecommended")]
        public string? TestRecommended { get; set; }

        [JsonProperty("remarks")]
        public string? Remarks { get; set; }
    }
}
