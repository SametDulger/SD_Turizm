using SD_Turizm.Web.Models.DTOs;

namespace SD_Turizm.Web.Services
{
    public class HotelApiService : IHotelApiService
    {
        private readonly IApiClientService _apiClient;

        public HotelApiService(IApiClientService apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<List<HotelDto>> GetAllHotelsAsync()
        {
            var response = await _apiClient.GetAsync<PaginatedResponse<HotelDto>>("Hotels");
            return response?.Items ?? new List<HotelDto>();
        }

        public async Task<HotelDto?> GetHotelByIdAsync(int id)
        {
            return await _apiClient.GetAsync<HotelDto>($"Hotels/{id}");
        }

        public async Task<HotelDto?> CreateHotelAsync(HotelDto hotel)
        {
            return await _apiClient.PostAsync<HotelDto>("Hotels", hotel);
        }

        public async Task<HotelDto?> UpdateHotelAsync(int id, HotelDto hotel)
        {
            return await _apiClient.PutAsync<HotelDto>($"Hotels/{id}", hotel);
        }

        public async Task<bool> DeleteHotelAsync(int id)
        {
            return await _apiClient.DeleteAsync($"Hotels/{id}");
        }
    }
}
