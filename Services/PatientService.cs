using ClinicQueue.Models;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace ClinicQueue.Services
{
    public interface IPatientService
    {
        Task<ApiResponse<Appointment>> BookAppointmentAsync(string token, BookAppointmentRequest request);
        Task<ApiResponse<List<Appointment>>> GetMyAppointmentsAsync(string token);
        Task<ApiResponse<Appointment>> GetAppointmentDetailsAsync(string token, int id);
        Task<ApiResponse<List<Prescription>>> GetMyPrescriptionsAsync(string token);
        Task<ApiResponse<List<Report>>> GetMyReportsAsync(string token);
    }

    public class PatientService : IPatientService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<PatientService> _logger;

        public PatientService(IHttpClientFactory httpClientFactory, ILogger<PatientService> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        public async Task<ApiResponse<Appointment>> BookAppointmentAsync(string token, BookAppointmentRequest request)
        {
            return await PostAsync<Appointment>("/appointments", token, request);
        }

        public async Task<ApiResponse<List<Appointment>>> GetMyAppointmentsAsync(string token)
        {
            return await GetAsync<List<Appointment>>("/appointments/my", token);
        }

        public async Task<ApiResponse<Appointment>> GetAppointmentDetailsAsync(string token, int id)
        {
            return await GetAsync<Appointment>($"/appointments/{id}", token);
        }

        public async Task<ApiResponse<List<Prescription>>> GetMyPrescriptionsAsync(string token)
        {
            return await GetAsync<List<Prescription>>("/prescriptions/my", token);
        }

        public async Task<ApiResponse<List<Report>>> GetMyReportsAsync(string token)
        {
            return await GetAsync<List<Report>>("/reports/my", token);
        }

        // ── Private helpers ──
        private async Task<ApiResponse<T>> GetAsync<T>(string endpoint, string token)
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

        private async Task<ApiResponse<T>> PostAsync<T>(string endpoint, string token, object data)
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
