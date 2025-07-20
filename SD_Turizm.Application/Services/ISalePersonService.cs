using SD_Turizm.Core.Entities;

namespace SD_Turizm.Application.Services
{
    public interface ISalePersonService
    {
        Task<IEnumerable<SalePerson>> GetAllAsync();
        Task<SalePerson?> GetByIdAsync(int id);
        Task<SalePerson> CreateAsync(SalePerson entity);
        Task<SalePerson> UpdateAsync(SalePerson entity);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
} 