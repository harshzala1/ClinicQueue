using Newtonsoft.Json;

namespace ClinicQueue.Models
{
    public class QueueEntry
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("tokenNumber")]
        public int TokenNumber { get; set; }

        [JsonProperty("status")]
        public string? Status { get; set; }

        [JsonProperty("queueDate")]
        public string? QueueDate { get; set; }

        [JsonProperty("appointmentId")]
        public int AppointmentId { get; set; }

        [JsonProperty("appointment")]
        public QueueAppointment? Appointment { get; set; }
    }

    public class QueueAppointment
    {
        [JsonProperty("patient")]
        public QueuePatient? Patient { get; set; }
    }

    public class QueuePatient
    {
        [JsonProperty("name")]
        public string? Name { get; set; }

        [JsonProperty("phone")]
        public string? Phone { get; set; }
    }

    public class DoctorQueueItem
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("tokenNumber")]
        public int TokenNumber { get; set; }

        [JsonProperty("status")]
        public string? Status { get; set; }

        [JsonProperty("patientName")]
        public string? PatientName { get; set; }

        [JsonProperty("patientId")]
        public int PatientId { get; set; }

        [JsonProperty("appointmentId")]
        public int AppointmentId { get; set; }
    }

    public class QueueUpdateRequest
    {
        [JsonProperty("status")]
        public string Status { get; set; } = string.Empty;
    }
}
