using SD_Turizm.Web.Models.DTOs;

namespace SD_Turizm.Web.Services
{
    public class CariTransactionApiService : ICariTransactionApiService
    {
        private readonly IApiClientService _apiClient;

        public CariTransactionApiService(IApiClientService apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<List<CariTransactionDto>?> GetAllTransactionsAsync()
        {
            return await _apiClient.GetAsync<List<CariTransactionDto>>("CariTransaction");
        }

        public async Task<CariTransactionDto?> GetTransactionByIdAsync(int id)
        {
            return await _apiClient.GetAsync<CariTransactionDto>($"CariTransaction/{id}");
        }

        public async Task<CariTransactionDto?> CreateTransactionAsync(CariTransactionDto transaction)
        {
            return await _apiClient.PostAsync<CariTransactionDto>("CariTransaction", transaction);
        }

        public async Task<CariTransactionDto?> UpdateTransactionAsync(int id, CariTransactionDto transaction)
        {
            return await _apiClient.PutAsync<CariTransactionDto>($"CariTransaction/{id}", transaction);
        }

        public async Task<bool> DeleteTransactionAsync(int id)
        {
            return await _apiClient.DeleteAsync($"CariTransaction/{id}");
        }

        public async Task<List<CariTransactionDto>?> GetTransactionsByVendorAsync(int vendorId)
        {
            return await _apiClient.GetAsync<List<CariTransactionDto>>($"CariTransaction/vendor/{vendorId}");
        }

        public async Task<List<CariTransactionDto>?> GetTransactionsByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _apiClient.GetAsync<List<CariTransactionDto>>($"CariTransaction/date-range?start={startDate:yyyy-MM-dd}&end={endDate:yyyy-MM-dd}");
        }

        public async Task<decimal> GetVendorBalanceAsync(int vendorId)
        {
            var result = await _apiClient.GetAsync<decimal>($"CariTransaction/vendor/{vendorId}/balance");
            return result;
        }
    }
}
