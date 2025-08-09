using SD_Turizm.Web.Models.DTOs;

namespace SD_Turizm.Web.Services
{
    public interface IUserApiService
    {
        Task<List<UserDto>?> GetAllUsersAsync();
        Task<UserDto?> GetUserByIdAsync(int id);
        Task<UserDto?> CreateUserAsync(UserDto user);
        Task<UserDto?> UpdateUserAsync(int id, UserDto user);
        Task<bool> DeleteUserAsync(int id);
        Task<bool> ChangePasswordAsync(int id, ChangePasswordDto changePassword);
        Task<List<UserDto>?> GetUsersByRoleAsync(string role);
        Task<bool> ActivateUserAsync(int id);
        Task<bool> DeactivateUserAsync(int id);
    }
}
