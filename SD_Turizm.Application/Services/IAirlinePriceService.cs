using System.Collections.Generic;
using System.Threading.Tasks;
using SD_Turizm.Core.Entities.Prices;
using SD_Turizm.Core.DTOs;

namespace SD_Turizm.Application.Services
{
    public interface IAirlinePriceService
    {
        Task<IEnumerable<AirlinePrice>> GetAllAsync();
        Task<AirlinePrice?> GetByIdAsync(int id);
        Task<AirlinePrice> CreateAsync(AirlinePrice airlinePrice);
        Task<AirlinePrice> UpdateAsync(AirlinePrice airlinePrice);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        
        // V2 Methods
        Task<PagedResult<AirlinePrice>> GetAirlinePricesWithPaginationAsync(PaginationDto pagination, int? airlineId = null, decimal? minPrice = null, decimal? maxPrice = null, DateTime? startDate = null, DateTime? endDate = null);
        Task<IEnumerable<AirlinePrice>> GetPricesByAirlineIdAsync(int airlineId);
        Task<object> GetPriceStatisticsAsync();
    }
} 
