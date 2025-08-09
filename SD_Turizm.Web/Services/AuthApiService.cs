using SD_Turizm.Web.Models.DTOs;

namespace SD_Turizm.Web.Services
{
    public class AuthApiService : IAuthApiService
    {
        private readonly IApiClientService _apiClient;

        public AuthApiService(IApiClientService apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<LoginResponseDto?> LoginAsync(LoginRequestDto loginRequest)
        {
            return await _apiClient.PostAsync<LoginResponseDto>("Auth/login", loginRequest);
        }

        public async Task<bool> LogoutAsync()
        {
            return await _apiClient.DeleteAsync("Auth/logout");
        }

        public async Task<UserDto?> GetCurrentUserAsync()
        {
            return await _apiClient.GetAsync<UserDto>("Auth/me");
        }
    }
}
