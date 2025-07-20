using System.Collections.Generic;
using System.Threading.Tasks;
using SD_Turizm.Core.Entities.Prices;

namespace SD_Turizm.Application.Services
{
    public interface IHotelPriceService
    {
        Task<IEnumerable<HotelPrice>> GetAllAsync();
        Task<HotelPrice> GetByIdAsync(int id);
        Task<HotelPrice> CreateAsync(HotelPrice entity);
        Task UpdateAsync(HotelPrice entity);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
} 