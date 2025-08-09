using SD_Turizm.Core.Entities;
using SD_Turizm.Core.DTOs;

namespace SD_Turizm.Application.Services
{
    public interface IPermissionService
    {
        Task<IEnumerable<Permission>> GetAllAsync();
        Task<Permission?> GetByIdAsync(int id);
        Task<Permission?> GetByNameAsync(string name);
        Task<Permission> CreateAsync(Permission permission);
        Task<Permission> UpdateAsync(Permission permission);
        Task DeleteAsync(int id);
        Task<bool> PermissionExistsAsync(int id);
        Task<bool> PermissionNameExistsAsync(string name);
        Task<IEnumerable<Permission>> GetByResourceAsync(string resource);
        Task<IEnumerable<Permission>> GetByActionAsync(string action);
        Task<PagedResult<Permission>> GetPagedAsync(int page, int pageSize, string? searchTerm = null);
        Task<IEnumerable<string>> GetAllResourcesAsync();
        Task<IEnumerable<string>> GetAllActionsAsync();
    }
}
