using SD_Turizm.Web.Models.DTOs;

namespace SD_Turizm.Web.Services
{
    public class TourPriceApiService : ITourPriceApiService
    {
        private readonly IApiClientService _apiClient;

        public TourPriceApiService(IApiClientService apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<List<TourPriceDto>?> GetAllTourPricesAsync()
        {
            return await _apiClient.GetAsync<List<TourPriceDto>>("TourPrice");
        }

        public async Task<TourPriceDto?> GetTourPriceByIdAsync(int id)
        {
            return await _apiClient.GetAsync<TourPriceDto>($"TourPrice/{id}");
        }

        public async Task<TourPriceDto?> CreateTourPriceAsync(TourPriceDto tourPrice)
        {
            return await _apiClient.PostAsync<TourPriceDto>("TourPrice", tourPrice);
        }

        public async Task<TourPriceDto?> UpdateTourPriceAsync(int id, TourPriceDto tourPrice)
        {
            return await _apiClient.PutAsync<TourPriceDto>($"TourPrice/{id}", tourPrice);
        }

        public async Task<bool> DeleteTourPriceAsync(int id)
        {
            return await _apiClient.DeleteAsync($"TourPrice/{id}");
        }

        public async Task<List<TourPriceDto>?> GetTourPricesByTourIdAsync(int tourId)
        {
            return await _apiClient.GetAsync<List<TourPriceDto>>($"TourPrice/tour/{tourId}");
        }
    }
}
