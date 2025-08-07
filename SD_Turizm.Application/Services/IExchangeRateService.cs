using System.Collections.Generic;
using System.Threading.Tasks;
using SD_Turizm.Core.Entities;

namespace SD_Turizm.Application.Services
{
    public interface IExchangeRateService
    {
        Task<IEnumerable<ExchangeRate>> GetAllAsync();
        Task<ExchangeRate> GetByIdAsync(int id);
        Task<ExchangeRate> CreateAsync(ExchangeRate entity);
        Task UpdateAsync(ExchangeRate entity);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
} 
