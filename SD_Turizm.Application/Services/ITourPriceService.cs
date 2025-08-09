using System.Collections.Generic;
using System.Threading.Tasks;
using SD_Turizm.Core.Entities.Prices;
using SD_Turizm.Core.DTOs;

namespace SD_Turizm.Application.Services
{
    public interface ITourPriceService
    {
        Task<IEnumerable<TourPrice>> GetAllAsync();
        Task<TourPrice?> GetByIdAsync(int id);
        Task<TourPrice> CreateAsync(TourPrice entity);
        Task UpdateAsync(TourPrice entity);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        
        // V2 Methods
        Task<PagedResult<TourPrice>> GetTourPricesWithPaginationAsync(PaginationDto pagination, int? tourId = null, decimal? minPrice = null, decimal? maxPrice = null, DateTime? startDate = null, DateTime? endDate = null);
        Task<IEnumerable<TourPrice>> GetPricesByTourIdAsync(int tourId);
        Task<object> GetPriceStatisticsAsync(int? tourId = null, DateTime? startDate = null, DateTime? endDate = null);
    }
} 
