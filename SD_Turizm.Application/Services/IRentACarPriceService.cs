using System.Collections.Generic;
using System.Threading.Tasks;
using SD_Turizm.Core.Entities.Prices;

namespace SD_Turizm.Application.Services
{
    public interface IRentACarPriceService
    {
        Task<IEnumerable<RentACarPrice>> GetAllAsync();
        Task<RentACarPrice> GetByIdAsync(int id);
        Task<RentACarPrice> CreateAsync(RentACarPrice entity);
        Task UpdateAsync(RentACarPrice entity);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
} 