using ClinicQueue.Models;
using ClinicQueue.Helpers;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace ClinicQueue.Services
{
    public interface IAdminService
    {
        Task<ApiResponse<ClinicInfo>> GetClinicInfoAsync(string token);
        Task<ApiResponse<List<User>>> GetUsersAsync(string token);
        Task<ApiResponse<User>> CreateUserAsync(string token, object userData);
    }

    public class AdminService : IAdminService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<AdminService> _logger;

        public AdminService(IHttpClientFactory httpClientFactory, ILogger<AdminService> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        public async Task<ApiResponse<ClinicInfo>> GetClinicInfoAsync(string token)
        {
            return await SendGetRequestAsync<ClinicInfo>("/admin/clinic", token);
        }

        public async Task<ApiResponse<List<User>>> GetUsersAsync(string token)
        {
            return await SendGetRequestAsync<List<User>>("/admin/users", token);
        }

        public async Task<ApiResponse<User>> CreateUserAsync(string token, object userData)
        {
            return await SendPostRequestAsync<User>("/admin/users", token, userData);
        }

        // ── Private helpers ──
        private async Task<ApiResponse<T>> SendGetRequestAsync<T>(string endpoint, string token)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("ClinicApi");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await client.GetAsync(endpoint);
                var body = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var data = JsonConvert.DeserializeObject<T>(body);
                    return new ApiResponse<T> { Success = true, Data = data, StatusCode = (int)response.StatusCode };
                }

                var error = JsonConvert.DeserializeObject<ApiError>(body);
                return new ApiResponse<T> { Success = false, Error = error?.Error ?? "Request failed", StatusCode = (int)response.StatusCode };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GET {Endpoint} failed", endpoint);
                return new ApiResponse<T> { Success = false, Error = "Server connection error", StatusCode = 500 };
            }
        }

        private async Task<ApiResponse<T>> SendPostRequestAsync<T>(string endpoint, string token, object data)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("ClinicApi");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var content = new StringContent(
                    JsonConvert.SerializeObject(data),
                    System.Text.Encoding.UTF8,
                    "application/json");

                var response = await client.PostAsync(endpoint, content);
                var body = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var result = JsonConvert.DeserializeObject<T>(body);
                    return new ApiResponse<T> { Success = true, Data = result, StatusCode = (int)response.StatusCode };
                }

                var error = JsonConvert.DeserializeObject<ApiError>(body);
                return new ApiResponse<T> { Success = false, Error = error?.Error ?? "Request failed", StatusCode = (int)response.StatusCode };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "POST {Endpoint} failed", endpoint);
                return new ApiResponse<T> { Success = false, Error = "Server connection error", StatusCode = 500 };
            }
        }
    }
}
