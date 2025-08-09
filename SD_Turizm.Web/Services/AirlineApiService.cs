using SD_Turizm.Web.Models.DTOs;

namespace SD_Turizm.Web.Services
{
    public class AirlineApiService : IAirlineApiService
    {
        private readonly IApiClientService _apiClient;

        public AirlineApiService(IApiClientService apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<List<AirlineDto>> GetAllAirlinesAsync()
        {
            var response = await _apiClient.GetAsync<PaginatedResponse<AirlineDto>>("Airline");
            return response?.Items ?? new List<AirlineDto>();
        }

        public async Task<AirlineDto?> GetAirlineByIdAsync(int id)
        {
            return await _apiClient.GetAsync<AirlineDto>($"Airline/{id}");
        }

        public async Task<AirlineDto?> CreateAirlineAsync(AirlineDto airline)
        {
            return await _apiClient.PostAsync<AirlineDto>("Airline", airline);
        }

        public async Task<AirlineDto?> UpdateAirlineAsync(int id, AirlineDto airline)
        {
            return await _apiClient.PutAsync<AirlineDto>($"Airline/{id}", airline);
        }

        public async Task<bool> DeleteAirlineAsync(int id)
        {
            return await _apiClient.DeleteAsync($"Airline/{id}");
        }
    }
}
