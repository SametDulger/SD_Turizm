using SD_Turizm.Web.Models.DTOs;

namespace SD_Turizm.Web.Services
{
    public class TransferPriceApiService : ITransferPriceApiService
    {
        private readonly IApiClientService _apiClient;

        public TransferPriceApiService(IApiClientService apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<List<TransferPriceDto>?> GetAllTransferPricesAsync()
        {
            return await _apiClient.GetAsync<List<TransferPriceDto>>("TransferPrice");
        }

        public async Task<TransferPriceDto?> GetTransferPriceByIdAsync(int id)
        {
            return await _apiClient.GetAsync<TransferPriceDto>($"TransferPrice/{id}");
        }

        public async Task<TransferPriceDto?> CreateTransferPriceAsync(TransferPriceDto transferPrice)
        {
            return await _apiClient.PostAsync<TransferPriceDto>("TransferPrice", transferPrice);
        }

        public async Task<TransferPriceDto?> UpdateTransferPriceAsync(int id, TransferPriceDto transferPrice)
        {
            return await _apiClient.PutAsync<TransferPriceDto>($"TransferPrice/{id}", transferPrice);
        }

        public async Task<bool> DeleteTransferPriceAsync(int id)
        {
            return await _apiClient.DeleteAsync($"TransferPrice/{id}");
        }

        public async Task<List<TransferPriceDto>?> GetTransferPricesByTransferCompanyIdAsync(int transferCompanyId)
        {
            return await _apiClient.GetAsync<List<TransferPriceDto>>($"TransferPrice/company/{transferCompanyId}");
        }
    }
}
