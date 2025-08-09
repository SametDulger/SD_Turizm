using SD_Turizm.Core.Entities;
using SD_Turizm.Core.DTOs;

namespace SD_Turizm.Application.Services
{
    public interface ITourService
    {
        Task<IEnumerable<Tour>> GetAllAsync();
        Task<Tour?> GetByIdAsync(int id);
        Task<Tour> CreateAsync(Tour entity);
        Task<Tour> UpdateAsync(Tour entity);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        
        // V2 Methods
        Task<PagedResult<Tour>> GetToursWithPaginationAsync(PaginationDto pagination, string? searchTerm = null, string? destination = null, int? duration = null, bool? isActive = null);
        Task<Tour?> GetByCodeAsync(string code);
        Task<PagedResult<Tour>> SearchToursAsync(PaginationDto pagination, string destination, int? duration = null, decimal? minPrice = null, decimal? maxPrice = null);
        Task<object> GetTourStatisticsAsync();
        Task<int> BulkUpdateAsync(List<Tour> tours);
    }
} 
