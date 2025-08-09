using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Options;

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
            AddAuthorizationHeader();
            return await ExecuteWithRetryAsync(() => _httpClient.GetAsync(endpoint));
        }

        public async Task<HttpResponseMessage> PostResponseAsync(string endpoint, object data)
        {
            AddAuthorizationHeader();
            var json = JsonSerializer.Serialize(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            return await ExecuteWithRetryAsync(() => _httpClient.PostAsync(endpoint, content));
        }

        public async Task<HttpResponseMessage> PutResponseAsync(string endpoint, object data)
        {
            AddAuthorizationHeader();
            var json = JsonSerializer.Serialize(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            return await ExecuteWithRetryAsync(() => _httpClient.PutAsync(endpoint, content));
        }

        public async Task<HttpResponseMessage> DeleteResponseAsync(string endpoint)
        {
            AddAuthorizationHeader();
            return await ExecuteWithRetryAsync(() => _httpClient.DeleteAsync(endpoint));
        }

        private void AddAuthorizationHeader()
        {
            try
            {
                var token = _httpContextAccessor.HttpContext?.Session.GetString("JWTToken");
                if (!string.IsNullOrEmpty(token))
                {
                    _httpClient.DefaultRequestHeaders.Authorization = 
                        new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
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
