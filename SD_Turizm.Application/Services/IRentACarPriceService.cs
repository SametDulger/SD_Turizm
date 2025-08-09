using System.Collections.Generic;
using System.Threading.Tasks;
using SD_Turizm.Core.Entities.Prices;
using SD_Turizm.Core.DTOs;

namespace SD_Turizm.Application.Services
{
    public interface IRentACarPriceService
    {
        Task<IEnumerable<RentACarPrice>> GetAllAsync();
        Task<RentACarPrice?> GetByIdAsync(int id);
        Task<RentACarPrice> CreateAsync(RentACarPrice rentACarPrice);
        Task<RentACarPrice> UpdateAsync(RentACarPrice rentACarPrice);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        
        // V2 Methods
        Task<PagedResult<RentACarPrice>> GetRentACarPricesWithPaginationAsync(PaginationDto pagination, int? rentACarId = null, decimal? minPrice = null, decimal? maxPrice = null, DateTime? startDate = null, DateTime? endDate = null);
        Task<IEnumerable<RentACarPrice>> GetPricesByRentACarIdAsync(int rentACarId);
        Task<object> GetPriceStatisticsAsync();
    }
} 
