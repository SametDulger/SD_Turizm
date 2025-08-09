using SD_Turizm.Web.Models.DTOs;

namespace SD_Turizm.Web.Services
{
    public class HotelPriceApiService : IHotelPriceApiService
    {
        private readonly IApiClientService _apiClient;

        public HotelPriceApiService(IApiClientService apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<List<HotelPriceDto>?> GetAllHotelPricesAsync()
        {
            return await _apiClient.GetAsync<List<HotelPriceDto>>("HotelPrice");
        }

        public async Task<HotelPriceDto?> GetHotelPriceByIdAsync(int id)
        {
            return await _apiClient.GetAsync<HotelPriceDto>($"HotelPrice/{id}");
        }

        public async Task<HotelPriceDto?> CreateHotelPriceAsync(HotelPriceDto hotelPrice)
        {
            return await _apiClient.PostAsync<HotelPriceDto>("HotelPrice", hotelPrice);
        }

        public async Task<HotelPriceDto?> UpdateHotelPriceAsync(int id, HotelPriceDto hotelPrice)
        {
            return await _apiClient.PutAsync<HotelPriceDto>($"HotelPrice/{id}", hotelPrice);
        }

        public async Task<bool> DeleteHotelPriceAsync(int id)
        {
            return await _apiClient.DeleteAsync($"HotelPrice/{id}");
        }

        public async Task<List<HotelPriceDto>?> GetHotelPricesByHotelIdAsync(int hotelId)
        {
            return await _apiClient.GetAsync<List<HotelPriceDto>>($"HotelPrice/hotel/{hotelId}");
        }
    }
}
