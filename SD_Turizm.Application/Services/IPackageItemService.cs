using SD_Turizm.Core.Entities;

namespace SD_Turizm.Application.Services
{
    public interface IPackageItemService
    {
        Task<IEnumerable<PackageItem>> GetAllAsync();
        Task<PackageItem?> GetByIdAsync(int id);
        Task<PackageItem> CreateAsync(PackageItem entity);
        Task<PackageItem> UpdateAsync(PackageItem entity);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
} 