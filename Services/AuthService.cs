using ClinicQueue.Models;
using ClinicQueue.Helpers;
using Newtonsoft.Json;

namespace ClinicQueue.Services
{
    public interface IAuthService
    {
        Task<ApiResponse<LoginResponse>> LoginAsync(string email, string password);
    }

    public class AuthService : IAuthService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<AuthService> _logger;

        public AuthService(IHttpClientFactory httpClientFactory, ILogger<AuthService> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        public async Task<ApiResponse<LoginResponse>> LoginAsync(string email, string password)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("ClinicApi");
                var requestBody = new { email, password };
                var settings = new JsonSerializerSettings
                {
                    ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver()
                };
                var content = new StringContent(
                    JsonConvert.SerializeObject(requestBody, settings),
                    System.Text.Encoding.UTF8,
                    "application/json");

                var response = await client.PostAsync("/auth/login", content);
                var responseBody = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var loginResponse = JsonConvert.DeserializeObject<LoginResponse>(responseBody);
                    return new ApiResponse<LoginResponse>
                    {
                        Success = true,
                        Data = loginResponse,
                        StatusCode = (int)response.StatusCode
                    };
                }

                var error = JsonConvert.DeserializeObject<ApiError>(responseBody);
                return new ApiResponse<LoginResponse>
                {
                    Success = false,
                    Error = error?.Error ?? "Login failed",
                    StatusCode = (int)response.StatusCode
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Login request failed");
                return new ApiResponse<LoginResponse>
                {
                    Success = false,
                    Error = "Unable to connect to the server. Please try again.",
                    StatusCode = 500
                };
            }
        }
    }
}
