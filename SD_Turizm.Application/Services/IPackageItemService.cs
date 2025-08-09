using System.Collections.Generic;
using System.Threading.Tasks;
using SD_Turizm.Core.Entities;
using SD_Turizm.Core.DTOs;

namespace SD_Turizm.Application.Services
{
    public interface IPackageItemService
    {
        Task<IEnumerable<PackageItem>> GetAllAsync();
        Task<PackageItem?> GetByIdAsync(int id);
        Task<PackageItem> CreateAsync(PackageItem packageItem);
        Task<PackageItem> UpdateAsync(PackageItem packageItem);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        
        // V2 Methods
        Task<PagedResult<PackageItem>> GetPackageItemsWithPaginationAsync(PaginationDto pagination, int? packageId = null, string? itemType = null, decimal? minPrice = null, decimal? maxPrice = null);
        Task<IEnumerable<PackageItem>> GetItemsByPackageIdAsync(int packageId);
        Task<object> GetPackageItemStatisticsAsync();
    }
} 
