using ClinicQueue.Models;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace ClinicQueue.Services
{
    public interface IDoctorService
    {
        Task<ApiResponse<List<DoctorQueueItem>>> GetTodayQueueAsync(string token);
        Task<ApiResponse<object>> AddPrescriptionAsync(string token, int appointmentId, AddPrescriptionRequest request);
        Task<ApiResponse<object>> AddReportAsync(string token, int appointmentId, AddReportRequest request);
    }

    public class DoctorService : IDoctorService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<DoctorService> _logger;

        public DoctorService(IHttpClientFactory httpClientFactory, ILogger<DoctorService> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        public async Task<ApiResponse<List<DoctorQueueItem>>> GetTodayQueueAsync(string token)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("ClinicApi");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await client.GetAsync("/doctor/queue");
                var body = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var data = JsonConvert.DeserializeObject<List<DoctorQueueItem>>(body);
                    return new ApiResponse<List<DoctorQueueItem>> { Success = true, Data = data, StatusCode = (int)response.StatusCode };
                }

                var error = JsonConvert.DeserializeObject<ApiError>(body);
                return new ApiResponse<List<DoctorQueueItem>> { Success = false, Error = error?.Error ?? "Request failed", StatusCode = (int)response.StatusCode };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GET /doctor/queue failed");
                return new ApiResponse<List<DoctorQueueItem>> { Success = false, Error = "Server connection error", StatusCode = 500 };
            }
        }

        public async Task<ApiResponse<object>> AddPrescriptionAsync(string token, int appointmentId, AddPrescriptionRequest request)
        {
            return await PostAsync($"/prescriptions/{appointmentId}", token, request);
        }

        public async Task<ApiResponse<object>> AddReportAsync(string token, int appointmentId, AddReportRequest request)
        {
            return await PostAsync($"/reports/{appointmentId}", token, request);
        }

        private async Task<ApiResponse<object>> PostAsync(string endpoint, string token, object data)
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
                    var result = JsonConvert.DeserializeObject<object>(body);
                    return new ApiResponse<object> { Success = true, Data = result, StatusCode = (int)response.StatusCode };
                }

                var error = JsonConvert.DeserializeObject<ApiError>(body);
                return new ApiResponse<object> { Success = false, Error = error?.Error ?? "Request failed", StatusCode = (int)response.StatusCode };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "POST {Endpoint} failed", endpoint);
                return new ApiResponse<object> { Success = false, Error = "Server connection error", StatusCode = 500 };
            }
        }
    }
}
