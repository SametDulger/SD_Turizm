using SD_Turizm.Web.Models.DTOs;

namespace SD_Turizm.Web.Services
{
    public class AuthApiService : IAuthApiService
    {
        private readonly IApiClientService _apiClient;
        private readonly ILogger<AuthApiService> _logger;

        public AuthApiService(IApiClientService apiClient, ILogger<AuthApiService> logger)
        {
            _apiClient = apiClient;
            _logger = logger;
        }

        public async Task<LoginResponseDto?> LoginAsync(string username, string password)
        {
            try
            {
                var loginRequest = new LoginRequestDto
                {
                    Username = username,
                    Password = password
                };

                return await _apiClient.PostAsync<LoginResponseDto>("Auth/login", loginRequest);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during login API call");
                return null;
            }
        }

        public async Task<RegisterResponseDto?> RegisterAsync(string username, string email, string password)
        {
            try
            {
                var registerRequest = new RegisterRequestDto
                {
                    Username = username,
                    Email = email,
                    Password = password
                };

                return await _apiClient.PostAsync<RegisterResponseDto>("Auth/register", registerRequest);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during register API call");
                return null;
            }
        }

        public async Task<LoginResponseDto?> RefreshTokenAsync(string refreshToken)
        {
            try
            {
                var refreshRequest = new RefreshTokenRequestDto
                {
                    RefreshToken = refreshToken
                };

                return await _apiClient.PostAsync<LoginResponseDto>("Auth/refresh", refreshRequest);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during refresh token API call");
                return null;
            }
        }

        public async Task<bool> ValidateTokenAsync(string token)
        {
            try
            {
                var response = await _apiClient.GetResponseAsync($"Auth/validate?token={token}");
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during token validation API call");
                return false;
            }
        }

        public Task LogoutAsync()
        {
            // JWT tokens don't require server-side logout
            // Just clear the client-side tokens
            return Task.CompletedTask;
        }
    }
}