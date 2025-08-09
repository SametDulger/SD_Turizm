using SD_Turizm.Core.Entities;
using SD_Turizm.Core.DTOs;

namespace SD_Turizm.Application.Services
{
    public interface IRoleService
    {
        Task<IEnumerable<Role>> GetAllAsync();
        Task<Role?> GetByIdAsync(int id);
        Task<Role?> GetByNameAsync(string name);
        Task<Role> CreateAsync(Role role);
        Task<Role> UpdateAsync(Role role);
        Task DeleteAsync(int id);
        Task<bool> RoleExistsAsync(int id);
        Task<bool> RoleNameExistsAsync(string name);
        Task<IEnumerable<Permission>> GetRolePermissionsAsync(int roleId);
        Task<bool> AssignPermissionAsync(int roleId, int permissionId);
        Task<bool> RemovePermissionAsync(int roleId, int permissionId);
        Task<bool> HasPermissionAsync(int roleId, string resource, string action);
        Task<PagedResult<Role>> GetPagedAsync(int page, int pageSize, string? searchTerm = null);
    }
}
