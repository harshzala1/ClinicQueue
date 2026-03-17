using Newtonsoft.Json;

namespace ClinicQueue.Models
{
    public class User
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; } = string.Empty;

        [JsonProperty("email")]
        public string Email { get; set; } = string.Empty;

        [JsonProperty("role")]
        public string Role { get; set; } = string.Empty;

        [JsonProperty("phone")]
        public string? Phone { get; set; }

        [JsonProperty("clinicId")]
        public int ClinicId { get; set; }

        [JsonProperty("clinicName")]
        public string? ClinicName { get; set; }

        [JsonProperty("clinicCode")]
        public string? ClinicCode { get; set; }

        [JsonProperty("createdAt")]
        public DateTime? CreatedAt { get; set; }
    }
}
