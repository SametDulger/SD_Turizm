using SD_Turizm.Web.Models.DTOs;

namespace SD_Turizm.Web.Services
{
    public class CruiseApiService : ICruiseApiService
    {
        private readonly IApiClientService _apiClient;

        public CruiseApiService(IApiClientService apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<List<CruiseDto>> GetAllCruisesAsync()
        {
            var response = await _apiClient.GetAsync<PaginatedResponse<CruiseDto>>("Cruise");
            return response?.Items ?? new List<CruiseDto>();
        }

        public async Task<CruiseDto?> GetCruiseByIdAsync(int id)
        {
            return await _apiClient.GetAsync<CruiseDto>($"Cruise/{id}");
        }

        public async Task<CruiseDto?> CreateCruiseAsync(CruiseDto cruise)
        {
            return await _apiClient.PostAsync<CruiseDto>("Cruise", cruise);
        }

        public async Task<CruiseDto?> UpdateCruiseAsync(int id, CruiseDto cruise)
        {
            return await _apiClient.PutAsync<CruiseDto>($"Cruise/{id}", cruise);
        }

        public async Task<bool> DeleteCruiseAsync(int id)
        {
            return await _apiClient.DeleteAsync($"Cruise/{id}");
        }
    }
}
