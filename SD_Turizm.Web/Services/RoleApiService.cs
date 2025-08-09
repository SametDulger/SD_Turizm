using SD_Turizm.Web.Models.DTOs;

namespace SD_Turizm.Web.Services
{
    public class RoleApiService : IRoleApiService
    {
        private readonly IApiClientService _apiClient;

        public RoleApiService(IApiClientService apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<List<RoleDto>?> GetAllRolesAsync()
        {
            return await _apiClient.GetAsync<List<RoleDto>>("Role");
        }

        public async Task<RoleDto?> GetRoleByIdAsync(int id)
        {
            return await _apiClient.GetAsync<RoleDto>($"Role/{id}");
        }

        public async Task<RoleDto?> CreateRoleAsync(RoleDto role)
        {
            return await _apiClient.PostAsync<RoleDto>("Role", role);
        }

        public async Task<RoleDto?> UpdateRoleAsync(int id, RoleDto role)
        {
            return await _apiClient.PutAsync<RoleDto>($"Role/{id}", role);
        }

        public async Task<bool> DeleteRoleAsync(int id)
        {
            return await _apiClient.DeleteAsync($"Role/{id}");
        }

        public async Task<List<PermissionDto>?> GetRolePermissionsAsync(int roleId)
        {
            return await _apiClient.GetAsync<List<PermissionDto>>($"Role/{roleId}/permissions");
        }

        public async Task<bool> AssignPermissionToRoleAsync(int roleId, int permissionId)
        {
            var result = await _apiClient.PostAsync<object>($"Role/{roleId}/permissions/{permissionId}", new { });
            return result != null;
        }

        public async Task<bool> RemovePermissionFromRoleAsync(int roleId, int permissionId)
        {
            return await _apiClient.DeleteAsync($"Role/{roleId}/permissions/{permissionId}");
        }
    }
}
