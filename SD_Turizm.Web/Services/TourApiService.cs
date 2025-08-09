using SD_Turizm.Web.Models.DTOs;

namespace SD_Turizm.Web.Services
{
    public class TourApiService : ITourApiService
    {
        private readonly IApiClientService _apiClient;

        public TourApiService(IApiClientService apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<List<TourDto>> GetAllToursAsync()
        {
            var response = await _apiClient.GetAsync<PaginatedResponse<TourDto>>("Tour");
            return response?.Items ?? new List<TourDto>();
        }

        public async Task<TourDto?> GetTourByIdAsync(int id)
        {
            return await _apiClient.GetAsync<TourDto>($"Tour/{id}");
        }

        public async Task<TourDto?> CreateTourAsync(TourDto tour)
        {
            return await _apiClient.PostAsync<TourDto>("Tour", tour);
        }

        public async Task<TourDto?> UpdateTourAsync(int id, TourDto tour)
        {
            return await _apiClient.PutAsync<TourDto>($"Tour/{id}", tour);
        }

        public async Task<bool> DeleteTourAsync(int id)
        {
            return await _apiClient.DeleteAsync($"Tour/{id}");
        }
    }
}
