using SD_Turizm.Web.Models.DTOs;

namespace SD_Turizm.Web.Services
{
    public interface IExchangeRateApiService
    {
        Task<List<ExchangeRateDto>?> GetAllExchangeRatesAsync();
        Task<ExchangeRateDto?> GetExchangeRateByIdAsync(int id);
        Task<ExchangeRateDto?> CreateExchangeRateAsync(ExchangeRateDto exchangeRate);
        Task<ExchangeRateDto?> UpdateExchangeRateAsync(int id, ExchangeRateDto exchangeRate);
        Task<bool> DeleteExchangeRateAsync(int id);
        Task<ExchangeRateDto?> GetLatestRateAsync(string fromCurrency, string toCurrency);
        Task<List<ExchangeRateDto>?> GetRatesByCurrencyAsync(string currency);
    }
}
