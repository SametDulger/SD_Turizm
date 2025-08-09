using System.Collections.Generic;
using System.Threading.Tasks;
using SD_Turizm.Core.Entities.Prices;
using SD_Turizm.Core.DTOs;

namespace SD_Turizm.Application.Services
{
    public interface IGuidePriceService
    {
        Task<IEnumerable<GuidePrice>> GetAllAsync();
        Task<GuidePrice?> GetByIdAsync(int id);
        Task<GuidePrice> CreateAsync(GuidePrice guidePrice);
        Task<GuidePrice> UpdateAsync(GuidePrice guidePrice);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        
        // V2 Methods
        Task<PagedResult<GuidePrice>> GetGuidePricesWithPaginationAsync(PaginationDto pagination, int? guideId = null, decimal? minPrice = null, decimal? maxPrice = null, DateTime? startDate = null, DateTime? endDate = null);
        Task<IEnumerable<GuidePrice>> GetPricesByGuideIdAsync(int guideId);
        Task<object> GetPriceStatisticsAsync();
    }
} 
