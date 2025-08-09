using SD_Turizm.Web.Models.DTOs;

namespace SD_Turizm.Web.Services
{
    public class RentACarPriceApiService : IRentACarPriceApiService
    {
        private readonly IApiClientService _apiClient;

        public RentACarPriceApiService(IApiClientService apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<List<RentACarPriceDto>?> GetAllRentACarPricesAsync()
        {
            return await _apiClient.GetAsync<List<RentACarPriceDto>>("RentACarPrice");
        }

        public async Task<RentACarPriceDto?> GetRentACarPriceByIdAsync(int id)
        {
            return await _apiClient.GetAsync<RentACarPriceDto>($"RentACarPrice/{id}");
        }

        public async Task<RentACarPriceDto?> CreateRentACarPriceAsync(RentACarPriceDto rentACarPrice)
        {
            return await _apiClient.PostAsync<RentACarPriceDto>("RentACarPrice", rentACarPrice);
        }

        public async Task<RentACarPriceDto?> UpdateRentACarPriceAsync(int id, RentACarPriceDto rentACarPrice)
        {
            return await _apiClient.PutAsync<RentACarPriceDto>($"RentACarPrice/{id}", rentACarPrice);
        }

        public async Task<bool> DeleteRentACarPriceAsync(int id)
        {
            return await _apiClient.DeleteAsync($"RentACarPrice/{id}");
        }

        public async Task<List<RentACarPriceDto>?> GetRentACarPricesByRentACarIdAsync(int rentACarId)
        {
            return await _apiClient.GetAsync<List<RentACarPriceDto>>($"RentACarPrice/rentacar/{rentACarId}");
        }
    }
}
