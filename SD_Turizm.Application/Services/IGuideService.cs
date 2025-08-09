using System.Collections.Generic;
using System.Threading.Tasks;
using SD_Turizm.Core.Entities;
using SD_Turizm.Core.DTOs;

namespace SD_Turizm.Application.Services
{
    public interface IGuideService
    {
        Task<IEnumerable<Guide>> GetAllAsync();
        Task<Guide?> GetByIdAsync(int id);
        Task<Guide> CreateAsync(Guide guide);
        Task<Guide> UpdateAsync(Guide guide);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        
        // V2 Methods
        Task<PagedResult<Guide>> GetGuidesWithPaginationAsync(PaginationDto pagination, string? searchTerm = null, string? region = null, bool? isActive = null);
        Task<PagedResult<Guide>> SearchGuidesAsync(PaginationDto pagination, string region, string? language = null);
        Task<object> GetGuideStatisticsAsync();
    }
} 
