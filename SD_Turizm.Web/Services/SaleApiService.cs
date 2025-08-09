using SD_Turizm.Web.Models.DTOs;

namespace SD_Turizm.Web.Services
{
    public class SaleApiService : ISaleApiService
    {
        private readonly IApiClientService _apiClient;

        public SaleApiService(IApiClientService apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<List<SaleDto>> GetAllSalesAsync()
        {
            var response = await _apiClient.GetAsync<PaginatedResponse<SaleDto>>("Sales");
            return response?.Items ?? new List<SaleDto>();
        }

        public async Task<SaleDto?> GetSaleByIdAsync(int id)
        {
            return await _apiClient.GetAsync<SaleDto>($"Sales/{id}");
        }

        public async Task<SaleDto?> CreateSaleAsync(SaleDto sale)
        {
            return await _apiClient.PostAsync<SaleDto>("Sales", sale);
        }

        public async Task<SaleDto?> UpdateSaleAsync(int id, SaleDto sale)
        {
            return await _apiClient.PutAsync<SaleDto>($"Sales/{id}", sale);
        }

        public async Task<bool> DeleteSaleAsync(int id)
        {
            return await _apiClient.DeleteAsync($"Sales/{id}");
        }
    }
}
