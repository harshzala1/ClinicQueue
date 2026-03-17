using Newtonsoft.Json;

namespace ClinicQueue.Models
{
    public class LoginRequest
    {
        [JsonProperty("email")]
        public string Email { get; set; } = string.Empty;

        [JsonProperty("password")]
        public string Password { get; set; } = string.Empty;
    }

    public class LoginResponse
    {
        [JsonProperty("token")]
        public string Token { get; set; } = string.Empty;

        [JsonProperty("user")]
        public User? User { get; set; }
    }
}
