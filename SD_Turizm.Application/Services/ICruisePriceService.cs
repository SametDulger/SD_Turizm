using System.Collections.Generic;
using System.Threading.Tasks;
using SD_Turizm.Core.Entities.Prices;
using SD_Turizm.Core.DTOs;

namespace SD_Turizm.Application.Services
{
    public interface ICruisePriceService
    {
        Task<IEnumerable<CruisePrice>> GetAllAsync();
        Task<CruisePrice?> GetByIdAsync(int id);
        Task<CruisePrice> CreateAsync(CruisePrice cruisePrice);
        Task<CruisePrice> UpdateAsync(CruisePrice cruisePrice);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        
        // V2 Methods
        Task<PagedResult<CruisePrice>> GetCruisePricesWithPaginationAsync(PaginationDto pagination, int? cruiseId = null, decimal? minPrice = null, decimal? maxPrice = null, DateTime? startDate = null, DateTime? endDate = null);
        Task<IEnumerable<CruisePrice>> GetPricesByCruiseIdAsync(int cruiseId);
        Task<object> GetPriceStatisticsAsync();
    }
} 
