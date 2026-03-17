using Newtonsoft.Json;

namespace ClinicQueue.Models
{
    public class ClinicInfo
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string? Name { get; set; }

        [JsonProperty("code")]
        public string? Code { get; set; }

        [JsonProperty("userCount")]
        public int UserCount { get; set; }

        [JsonProperty("appointmentCount")]
        public int AppointmentCount { get; set; }

        [JsonProperty("queueCount")]
        public int QueueCount { get; set; }
    }
}
