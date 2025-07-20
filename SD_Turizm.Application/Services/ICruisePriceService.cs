using System.Collections.Generic;
using System.Threading.Tasks;
using SD_Turizm.Core.Entities.Prices;

namespace SD_Turizm.Application.Services
{
    public interface ICruisePriceService
    {
        Task<IEnumerable<CruisePrice>> GetAllAsync();
        Task<CruisePrice> GetByIdAsync(int id);
        Task<CruisePrice> CreateAsync(CruisePrice entity);
        Task UpdateAsync(CruisePrice entity);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
} 