using SD_Turizm.Web.Models.DTOs;

namespace SD_Turizm.Web.Services
{
    public class TransferCompanyApiService : ITransferCompanyApiService
    {
        private readonly IApiClientService _apiClient;

        public TransferCompanyApiService(IApiClientService apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<List<TransferCompanyDto>> GetAllTransferCompaniesAsync()
        {
            var response = await _apiClient.GetAsync<PaginatedResponse<TransferCompanyDto>>("TransferCompany");
            return response?.Items ?? new List<TransferCompanyDto>();
        }

        public async Task<TransferCompanyDto?> GetTransferCompanyByIdAsync(int id)
        {
            return await _apiClient.GetAsync<TransferCompanyDto>($"TransferCompany/{id}");
        }

        public async Task<TransferCompanyDto?> CreateTransferCompanyAsync(TransferCompanyDto transferCompany)
        {
            return await _apiClient.PostAsync<TransferCompanyDto>("TransferCompany", transferCompany);
        }

        public async Task<TransferCompanyDto?> UpdateTransferCompanyAsync(int id, TransferCompanyDto transferCompany)
        {
            return await _apiClient.PutAsync<TransferCompanyDto>($"TransferCompany/{id}", transferCompany);
        }

        public async Task<bool> DeleteTransferCompanyAsync(int id)
        {
            return await _apiClient.DeleteAsync($"TransferCompany/{id}");
        }
    }
}
