using System.Collections.Generic;
using System.Threading.Tasks;
using SD_Turizm.Core.Entities;
using SD_Turizm.Core.DTOs;

namespace SD_Turizm.Application.Services
{
    public interface IPackageService
    {
        Task<IEnumerable<Package>> GetAllAsync();
        Task<Package?> GetByIdAsync(int id);
        Task<Package> CreateAsync(Package package);
        Task<Package> UpdateAsync(Package package);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        
        // V2 Methods
        Task<PagedResult<Package>> GetPackagesWithPaginationAsync(PaginationDto pagination, string? searchTerm = null, string? destination = null, bool? isActive = null);
        Task<PagedResult<Package>> SearchPackagesAsync(PaginationDto pagination, string destination, string? packageType = null);
        Task<object> GetPackageStatisticsAsync();
    }
} 
