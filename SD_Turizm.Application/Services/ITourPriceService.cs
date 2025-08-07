using System.Collections.Generic;
using System.Threading.Tasks;
using SD_Turizm.Core.Entities.Prices;

namespace SD_Turizm.Application.Services
{
    public interface ITourPriceService
    {
        Task<IEnumerable<TourPrice>> GetAllAsync();
        Task<TourPrice> GetByIdAsync(int id);
        Task<TourPrice> CreateAsync(TourPrice entity);
        Task UpdateAsync(TourPrice entity);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
} 
