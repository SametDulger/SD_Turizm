using System.Collections.Generic;
using System.Threading.Tasks;
using SD_Turizm.Core.Entities.Prices;

namespace SD_Turizm.Application.Services
{
    public interface IAirlinePriceService
    {
        Task<IEnumerable<AirlinePrice>> GetAllAsync();
        Task<AirlinePrice> GetByIdAsync(int id);
        Task<AirlinePrice> CreateAsync(AirlinePrice entity);
        Task UpdateAsync(AirlinePrice entity);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
} 