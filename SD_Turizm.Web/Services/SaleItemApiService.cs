using SD_Turizm.Web.Models.DTOs;

namespace SD_Turizm.Web.Services
{
    public class SaleItemApiService : ISaleItemApiService
    {
        private readonly IApiClientService _apiClient;

        public SaleItemApiService(IApiClientService apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<List<SaleItemDto>?> GetAllSaleItemsAsync()
        {
            return await _apiClient.GetAsync<List<SaleItemDto>>("SaleItem");
        }

        public async Task<SaleItemDto?> GetSaleItemByIdAsync(int id)
        {
            return await _apiClient.GetAsync<SaleItemDto>($"SaleItem/{id}");
        }

        public async Task<SaleItemDto?> CreateSaleItemAsync(SaleItemDto saleItem)
        {
            return await _apiClient.PostAsync<SaleItemDto>("SaleItem", saleItem);
        }

        public async Task<SaleItemDto?> UpdateSaleItemAsync(int id, SaleItemDto saleItem)
        {
            return await _apiClient.PutAsync<SaleItemDto>($"SaleItem/{id}", saleItem);
        }

        public async Task<bool> DeleteSaleItemAsync(int id)
        {
            return await _apiClient.DeleteAsync($"SaleItem/{id}");
        }

        public async Task<List<SaleItemDto>?> GetSaleItemsBySaleIdAsync(int saleId)
        {
            return await _apiClient.GetAsync<List<SaleItemDto>>($"SaleItem/sale/{saleId}");
        }
    }
}
