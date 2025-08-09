using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Options;
using SD_Turizm.Web.Models;

namespace SD_Turizm.Web.Services
{
    public class ApiClientService : IApiClientService
    {
        private readonly HttpClient _httpClient;
        private readonly ApiSettings _apiSettings;
        private readonly ILogger<ApiClientService> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ApiClientService(HttpClient httpClient, IOptions<ApiSettings> apiSettings, ILogger<ApiClientService> logger, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _apiSettings = apiSettings.Value;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;

            _httpClient.Timeout = TimeSpan.FromSeconds(_apiSettings.TimeoutSeconds);
            _httpClient.BaseAddress = new Uri(_apiSettings.BaseUrl);
            
            // Default headers
            _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
        }

        public async Task<T?> GetAsync<T>(string endpoint)
        {
            try
            {
                var response = await GetResponseAsync(endpoint);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<T>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GET request to {Endpoint}", endpoint);
            }
            return default;
        }

        public async Task<T?> PostAsync<T>(string endpoint, object data)
        {
            try
            {
                var response = await PostResponseAsync(endpoint, data);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<T>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in POST request to {Endpoint}", endpoint);
            }
            return default;
        }

        public async Task<T?> PutAsync<T>(string endpoint, object data)
        {
            try
            {
                var response = await PutResponseAsync(endpoint, data);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<T>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in PUT request to {Endpoint}", endpoint);
            }
            return default;
        }

        public async Task<bool> DeleteAsync(string endpoint)
        {
            try
            {
                var response = await DeleteResponseAsync(endpoint);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in DELETE request to {Endpoint}", endpoint);
                return false;
            }
        }

        public async Task<HttpResponseMessage> GetResponseAsync(string endpoint)
        {
            return await ExecuteWithRetryAsync(async () => {
                using var request = new HttpRequestMessage(HttpMethod.Get, endpoint);
                AddAuthToRequest(request);
                return await _httpClient.SendAsync(request);
            });
        }

        public async Task<HttpResponseMessage> PostResponseAsync(string endpoint, object data)
        {
            return await ExecuteWithRetryAsync(async () => {
                using var request = new HttpRequestMessage(HttpMethod.Post, endpoint);
                var json = JsonSerializer.Serialize(data);
                request.Content = new StringContent(json, Encoding.UTF8, "application/json");
                AddAuthToRequest(request);
                return await _httpClient.SendAsync(request);
            });
        }

        public async Task<HttpResponseMessage> PutResponseAsync(string endpoint, object data)
        {
            return await ExecuteWithRetryAsync(async () => {
                using var request = new HttpRequestMessage(HttpMethod.Put, endpoint);
                var json = JsonSerializer.Serialize(data);
                request.Content = new StringContent(json, Encoding.UTF8, "application/json");
                AddAuthToRequest(request);
                return await _httpClient.SendAsync(request);
            });
        }

        public async Task<HttpResponseMessage> DeleteResponseAsync(string endpoint)
        {
            return await ExecuteWithRetryAsync(async () => {
                using var request = new HttpRequestMessage(HttpMethod.Delete, endpoint);
                AddAuthToRequest(request);
                return await _httpClient.SendAsync(request);
            });
        }

        private void AddAuthToRequest(HttpRequestMessage request)
        {
            try
            {
                var token = _httpContextAccessor.HttpContext?.Session.GetString("JWTToken");
                if (!string.IsNullOrEmpty(token))
                {
                    request.Headers.Authorization = 
                        new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                    
                    _logger.LogWarning("🚀 MANUAL AUTH HEADER INJECTION: {TokenPreview}", 
                        token.Length > 20 ? token.Substring(0, 20) + "..." : token);
                }
                else
                {
                    _logger.LogWarning("⚠️ NO TOKEN FOUND IN SESSION!");
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "JWT token eklenirken hata oluştu");
            }
        }

        private async Task<HttpResponseMessage> ExecuteWithRetryAsync(Func<Task<HttpResponseMessage>> operation)
        {
            for (int i = 0; i <= _apiSettings.RetryCount; i++)
            {
                try
                {
                    var response = await operation();
                    return response;
                }
                catch (Exception ex) when (i < _apiSettings.RetryCount)
                {
                    _logger.LogWarning(ex, "Retry {RetryCount} for API request", i + 1);
                    await Task.Delay(1000 * (i + 1)); // Exponential backoff
                }
            }
            throw new HttpRequestException("API request failed after all retries");
        }
    }
}
