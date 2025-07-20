using System.Collections.Generic;
using System.Threading.Tasks;
using SD_Turizm.Core.Entities;

namespace SD_Turizm.Application.Services
{
    public interface IGuideService
    {
        Task<IEnumerable<Guide>> GetAllAsync();
        Task<Guide> GetByIdAsync(int id);
        Task<Guide> CreateAsync(Guide entity);
        Task UpdateAsync(Guide entity);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
} 