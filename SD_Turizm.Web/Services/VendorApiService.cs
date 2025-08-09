using SD_Turizm.Web.Models.DTOs;

namespace SD_Turizm.Web.Services
{
    public class VendorApiService : IVendorApiService
    {
        private readonly IApiClientService _apiClient;

        public VendorApiService(IApiClientService apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<List<VendorDto>> GetAllVendorsAsync()
        {
            return await _apiClient.GetAsync<List<VendorDto>>("Vendor") ?? new List<VendorDto>();
        }

        public async Task<List<VendorDto>> GetActiveVendorsAsync()
        {
            return await _apiClient.GetAsync<List<VendorDto>>("Vendor/active") ?? new List<VendorDto>();
        }

        public async Task<VendorDto?> GetVendorByIdAsync(int id)
        {
            return await _apiClient.GetAsync<VendorDto>($"Vendor/{id}");
        }

        public async Task<VendorDto?> CreateVendorAsync(VendorDto vendor)
        {
            return await _apiClient.PostAsync<VendorDto>("Vendor", vendor);
        }

        public async Task<VendorDto?> UpdateVendorAsync(int id, VendorDto vendor)
        {
            return await _apiClient.PutAsync<VendorDto>($"Vendor/{id}", vendor);
        }

        public async Task<bool> DeleteVendorAsync(int id)
        {
            return await _apiClient.DeleteAsync($"Vendor/{id}");
        }
    }
}
