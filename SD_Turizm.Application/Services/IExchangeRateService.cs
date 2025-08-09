using System.Collections.Generic;
using System.Threading.Tasks;
using SD_Turizm.Core.Entities;
using SD_Turizm.Core.DTOs;

namespace SD_Turizm.Application.Services
{
    public interface IExchangeRateService
    {
        Task<IEnumerable<ExchangeRate>> GetAllAsync();
        Task<ExchangeRate?> GetByIdAsync(int id);
        Task<ExchangeRate> CreateAsync(ExchangeRate exchangeRate);
        Task<ExchangeRate> UpdateAsync(ExchangeRate exchangeRate);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        
        // V2 Methods
        Task<PagedResult<ExchangeRate>> GetExchangeRatesWithPaginationAsync(PaginationDto pagination, string? fromCurrency = null, string? toCurrency = null, DateTime? startDate = null, DateTime? endDate = null);
        Task<ExchangeRate?> GetLatestRateAsync(string fromCurrency, string toCurrency);
        Task<object> GetExchangeRateStatisticsAsync();
    }
} 
