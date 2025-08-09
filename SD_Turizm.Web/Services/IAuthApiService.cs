using SD_Turizm.Web.Models.DTOs;

namespace SD_Turizm.Web.Services
{
    public interface IAuthApiService
    {
        Task<LoginResponseDto?> LoginAsync(string username, string password);
        Task<RegisterResponseDto?> RegisterAsync(string username, string email, string password);
        Task<LoginResponseDto?> RefreshTokenAsync(string refreshToken);
        Task<bool> ValidateTokenAsync(string token);
        Task LogoutAsync();
    }
}