using SD_Turizm.Web.Models.DTOs;

namespace SD_Turizm.Web.Services
{
    public class UserApiService : IUserApiService
    {
        private readonly IApiClientService _apiClient;

        public UserApiService(IApiClientService apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<List<UserDto>?> GetAllUsersAsync()
        {
            return await _apiClient.GetAsync<List<UserDto>>("User");
        }

        public async Task<UserDto?> GetUserByIdAsync(int id)
        {
            return await _apiClient.GetAsync<UserDto>($"User/{id}");
        }

        public async Task<UserDto?> CreateUserAsync(UserDto user)
        {
            return await _apiClient.PostAsync<UserDto>("User", user);
        }

        public async Task<UserDto?> UpdateUserAsync(int id, UserDto user)
        {
            return await _apiClient.PutAsync<UserDto>($"User/{id}", user);
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            return await _apiClient.DeleteAsync($"User/{id}");
        }

        public async Task<bool> ChangePasswordAsync(int id, ChangePasswordDto changePassword)
        {
            var result = await _apiClient.PostAsync<object>($"User/{id}/change-password", changePassword);
            return result != null;
        }

        public async Task<List<UserDto>?> GetUsersByRoleAsync(string role)
        {
            return await _apiClient.GetAsync<List<UserDto>>($"User/role/{role}");
        }

        public async Task<bool> ActivateUserAsync(int id)
        {
            var result = await _apiClient.PostAsync<object>($"User/{id}/activate", new { });
            return result != null;
        }

        public async Task<bool> DeactivateUserAsync(int id)
        {
            var result = await _apiClient.PostAsync<object>($"User/{id}/deactivate", new { });
            return result != null;
        }
    }
}
