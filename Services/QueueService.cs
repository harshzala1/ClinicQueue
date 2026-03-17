using ClinicQueue.Models;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace ClinicQueue.Services
{
    public interface IQueueService
    {
        Task<ApiResponse<List<QueueEntry>>> GetQueueAsync(string token, string date);
        Task<ApiResponse<QueueEntry>> UpdateQueueStatusAsync(string token, int id, string status);
    }

    public class QueueService : IQueueService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<QueueService> _logger;

        public QueueService(IHttpClientFactory httpClientFactory, ILogger<QueueService> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        public async Task<ApiResponse<List<QueueEntry>>> GetQueueAsync(string token, string date)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("ClinicApi");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await client.GetAsync($"/queue?date={date}");
                var body = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var data = JsonConvert.DeserializeObject<List<QueueEntry>>(body);
                    return new ApiResponse<List<QueueEntry>> { Success = true, Data = data, StatusCode = (int)response.StatusCode };
                }

                var error = JsonConvert.DeserializeObject<ApiError>(body);
                return new ApiResponse<List<QueueEntry>> { Success = false, Error = error?.Error ?? "Request failed", StatusCode = (int)response.StatusCode };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GET /queue failed");
                return new ApiResponse<List<QueueEntry>> { Success = false, Error = "Server connection error", StatusCode = 500 };
            }
        }

        public async Task<ApiResponse<QueueEntry>> UpdateQueueStatusAsync(string token, int id, string status)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("ClinicApi");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var payload = new QueueUpdateRequest { Status = status };
                var content = new StringContent(
                    JsonConvert.SerializeObject(payload),
                    System.Text.Encoding.UTF8,
                    "application/json");

                var request = new HttpRequestMessage(new HttpMethod("PATCH"), $"/queue/{id}")
                {
                    Content = content
                };

                var response = await client.SendAsync(request);
                var body = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var data = JsonConvert.DeserializeObject<QueueEntry>(body);
                    return new ApiResponse<QueueEntry> { Success = true, Data = data, StatusCode = (int)response.StatusCode };
                }

                var error = JsonConvert.DeserializeObject<ApiError>(body);
                return new ApiResponse<QueueEntry> { Success = false, Error = error?.Error ?? "Update failed", StatusCode = (int)response.StatusCode };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "PATCH /queue/{Id} failed", id);
                return new ApiResponse<QueueEntry> { Success = false, Error = "Server connection error", StatusCode = 500 };
            }
        }
    }
}
