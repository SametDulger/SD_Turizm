using SD_Turizm.Web.Models.DTOs;

namespace SD_Turizm.Web.Services
{
    public class CruisePriceApiService : ICruisePriceApiService
    {
        private readonly IApiClientService _apiClient;

        public CruisePriceApiService(IApiClientService apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<List<CruisePriceDto>?> GetAllCruisePricesAsync()
        {
            return await _apiClient.GetAsync<List<CruisePriceDto>>("CruisePrice");
        }

        public async Task<CruisePriceDto?> GetCruisePriceByIdAsync(int id)
        {
            return await _apiClient.GetAsync<CruisePriceDto>($"CruisePrice/{id}");
        }

        public async Task<CruisePriceDto?> CreateCruisePriceAsync(CruisePriceDto cruisePrice)
        {
            return await _apiClient.PostAsync<CruisePriceDto>("CruisePrice", cruisePrice);
        }

        public async Task<CruisePriceDto?> UpdateCruisePriceAsync(int id, CruisePriceDto cruisePrice)
        {
            return await _apiClient.PutAsync<CruisePriceDto>($"CruisePrice/{id}", cruisePrice);
        }

        public async Task<bool> DeleteCruisePriceAsync(int id)
        {
            return await _apiClient.DeleteAsync($"CruisePrice/{id}");
        }

        public async Task<List<CruisePriceDto>?> GetCruisePricesByCruiseIdAsync(int cruiseId)
        {
            return await _apiClient.GetAsync<List<CruisePriceDto>>($"CruisePrice/cruise/{cruiseId}");
        }
    }
}
