using SD_Turizm.Web.Models.DTOs;

namespace SD_Turizm.Web.Services
{
    public class AirlinePriceApiService : IAirlinePriceApiService
    {
        private readonly IApiClientService _apiClient;

        public AirlinePriceApiService(IApiClientService apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<List<AirlinePriceDto>?> GetAllAirlinePricesAsync()
        {
            return await _apiClient.GetAsync<List<AirlinePriceDto>>("AirlinePrice");
        }

        public async Task<AirlinePriceDto?> GetAirlinePriceByIdAsync(int id)
        {
            return await _apiClient.GetAsync<AirlinePriceDto>($"AirlinePrice/{id}");
        }

        public async Task<AirlinePriceDto?> CreateAirlinePriceAsync(AirlinePriceDto airlinePrice)
        {
            return await _apiClient.PostAsync<AirlinePriceDto>("AirlinePrice", airlinePrice);
        }

        public async Task<AirlinePriceDto?> UpdateAirlinePriceAsync(int id, AirlinePriceDto airlinePrice)
        {
            return await _apiClient.PutAsync<AirlinePriceDto>($"AirlinePrice/{id}", airlinePrice);
        }

        public async Task<bool> DeleteAirlinePriceAsync(int id)
        {
            return await _apiClient.DeleteAsync($"AirlinePrice/{id}");
        }

        public async Task<List<AirlinePriceDto>?> GetAirlinePricesByAirlineIdAsync(int airlineId)
        {
            return await _apiClient.GetAsync<List<AirlinePriceDto>>($"AirlinePrice/airline/{airlineId}");
        }
    }
}
