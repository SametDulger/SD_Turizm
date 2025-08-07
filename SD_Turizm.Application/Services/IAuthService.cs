using SD_Turizm.Core.DTOs;

namespace SD_Turizm.Application.Services
{
    public interface IAuthService
    {
        Task<AuthResponseDto> LoginAsync(LoginDto loginDto);
        Task<AuthResponseDto> RegisterAsync(RegisterDto registerDto);
        Task<AuthResponseDto> RefreshTokenAsync(string refreshToken);
        Task<bool> ValidateTokenAsync(string token);
        Task<UserDto> GetCurrentUserAsync();
        Task LogoutAsync();
    }
} 