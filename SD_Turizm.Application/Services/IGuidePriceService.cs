using System.Collections.Generic;
using System.Threading.Tasks;
using SD_Turizm.Core.Entities.Prices;

namespace SD_Turizm.Application.Services
{
    public interface IGuidePriceService
    {
        Task<IEnumerable<GuidePrice>> GetAllAsync();
        Task<GuidePrice> GetByIdAsync(int id);
        Task<GuidePrice> CreateAsync(GuidePrice entity);
        Task UpdateAsync(GuidePrice entity);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
} 
