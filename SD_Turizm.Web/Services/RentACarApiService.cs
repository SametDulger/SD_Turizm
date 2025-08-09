using SD_Turizm.Web.Models.DTOs;

namespace SD_Turizm.Web.Services
{
    public class RentACarApiService : IRentACarApiService
    {
        private readonly IApiClientService _apiClient;

        public RentACarApiService(IApiClientService apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<List<RentACarDto>> GetAllRentACarsAsync()
        {
            var response = await _apiClient.GetAsync<PaginatedResponse<RentACarDto>>("RentACar");
            return response?.Items ?? new List<RentACarDto>();
        }

        public async Task<RentACarDto?> GetRentACarByIdAsync(int id)
        {
            return await _apiClient.GetAsync<RentACarDto>($"RentACar/{id}");
        }

        public async Task<RentACarDto?> CreateRentACarAsync(RentACarDto rentACar)
        {
            return await _apiClient.PostAsync<RentACarDto>("RentACar", rentACar);
        }

        public async Task<RentACarDto?> UpdateRentACarAsync(int id, RentACarDto rentACar)
        {
            return await _apiClient.PutAsync<RentACarDto>($"RentACar/{id}", rentACar);
        }

        public async Task<bool> DeleteRentACarAsync(int id)
        {
            return await _apiClient.DeleteAsync($"RentACar/{id}");
        }
    }
}
