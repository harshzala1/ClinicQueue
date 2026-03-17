using Newtonsoft.Json;

namespace ClinicQueue.Models
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public T? Data { get; set; }
        public string? Error { get; set; }
        public int StatusCode { get; set; }
    }

    public class ApiError
    {
        [JsonProperty("error")]
        public string? Error { get; set; }
    }
}
