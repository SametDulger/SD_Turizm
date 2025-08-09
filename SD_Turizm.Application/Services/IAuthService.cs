using SD_Turizm.Core.DTOs;

namespace SD_Turizm.Application.Services
{
    public interface IAuthService
    {
        Task<LoginResponseDto> LoginAsync(string username, string password);
        Task<RegisterResponseDto> RegisterAsync(string username, string email, string password);
        Task<LoginResponseDto> RefreshTokenAsync(string refreshToken);
        Task<bool> ValidateTokenAsync(string token);
        Task<object> GetCurrentUserAsync();
        Task LogoutAsync();
        
        // V2 Methods
        Task<object> Enable2FAAsync();
        Task<bool> Verify2FAAsync(string code);
        Task<IEnumerable<object>> GetRolesAsync();
        Task<bool> AssignRoleAsync(int userId, int roleId);
        Task<IEnumerable<object>> GetUserSessionsAsync();
        Task<bool> RevokeSessionAsync(string sessionId);
        Task<bool> ChangePasswordAsync(string currentPassword, string newPassword);
        Task<bool> ForgotPasswordAsync(string email);
    }
} 