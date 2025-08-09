using SD_Turizm.Web.Models.DTOs;

namespace SD_Turizm.Web.Services
{
    public class ExchangeRateApiService : IExchangeRateApiService
    {
        private readonly IApiClientService _apiClient;

        public ExchangeRateApiService(IApiClientService apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<List<ExchangeRateDto>?> GetAllExchangeRatesAsync()
        {
            return await _apiClient.GetAsync<List<ExchangeRateDto>>("ExchangeRate");
        }

        public async Task<ExchangeRateDto?> GetExchangeRateByIdAsync(int id)
        {
            return await _apiClient.GetAsync<ExchangeRateDto>($"ExchangeRate/{id}");
        }

        public async Task<ExchangeRateDto?> CreateExchangeRateAsync(ExchangeRateDto exchangeRate)
        {
            return await _apiClient.PostAsync<ExchangeRateDto>("ExchangeRate", exchangeRate);
        }

        public async Task<ExchangeRateDto?> UpdateExchangeRateAsync(int id, ExchangeRateDto exchangeRate)
        {
            return await _apiClient.PutAsync<ExchangeRateDto>($"ExchangeRate/{id}", exchangeRate);
        }

        public async Task<bool> DeleteExchangeRateAsync(int id)
        {
            return await _apiClient.DeleteAsync($"ExchangeRate/{id}");
        }

        public async Task<ExchangeRateDto?> GetLatestRateAsync(string fromCurrency, string toCurrency)
        {
            return await _apiClient.GetAsync<ExchangeRateDto>($"ExchangeRate/latest/{fromCurrency}/{toCurrency}");
        }

        public async Task<List<ExchangeRateDto>?> GetRatesByCurrencyAsync(string currency)
        {
            return await _apiClient.GetAsync<List<ExchangeRateDto>>($"ExchangeRate/currency/{currency}");
        }
    }
}
