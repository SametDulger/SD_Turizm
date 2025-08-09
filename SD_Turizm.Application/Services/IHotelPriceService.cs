using System.Collections.Generic;
using System.Threading.Tasks;
using SD_Turizm.Core.Entities.Prices;
using SD_Turizm.Core.DTOs;

namespace SD_Turizm.Application.Services
{
    public interface IHotelPriceService
    {
        Task<IEnumerable<HotelPrice>> GetAllAsync();
        Task<HotelPrice?> GetByIdAsync(int id);
        Task<HotelPrice> CreateAsync(HotelPrice hotelPrice);
        Task<HotelPrice> UpdateAsync(HotelPrice hotelPrice);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        
        // V2 Methods
        Task<PagedResult<HotelPrice>> GetHotelPricesWithPaginationAsync(PaginationDto pagination, int? hotelId = null, decimal? minPrice = null, decimal? maxPrice = null, DateTime? startDate = null, DateTime? endDate = null);
        Task<IEnumerable<HotelPrice>> GetPricesByHotelIdAsync(int hotelId);
        Task<object> GetPriceStatisticsAsync();
    }
} 
