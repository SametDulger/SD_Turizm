using SD_Turizm.Core.Entities;

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
    }
} 