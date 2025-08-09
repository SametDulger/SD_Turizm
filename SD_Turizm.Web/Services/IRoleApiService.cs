using SD_Turizm.Web.Models.DTOs;

namespace SD_Turizm.Web.Services
{
    public interface IRoleApiService
    {
        Task<List<RoleDto>?> GetAllRolesAsync();
        Task<RoleDto?> GetRoleByIdAsync(int id);
        Task<RoleDto?> CreateRoleAsync(RoleDto role);
        Task<RoleDto?> UpdateRoleAsync(int id, RoleDto role);
        Task<bool> DeleteRoleAsync(int id);
        Task<List<PermissionDto>?> GetRolePermissionsAsync(int roleId);
        Task<bool> AssignPermissionToRoleAsync(int roleId, int permissionId);
        Task<bool> RemovePermissionFromRoleAsync(int roleId, int permissionId);
    }
}
