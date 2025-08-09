using SD_Turizm.Web.Models.DTOs;

namespace SD_Turizm.Web.Services
{
    public class PackageItemApiService : IPackageItemApiService
    {
        private readonly IApiClientService _apiClient;

        public PackageItemApiService(IApiClientService apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<List<PackageItemDto>?> GetAllPackageItemsAsync()
        {
            return await _apiClient.GetAsync<List<PackageItemDto>>("PackageItem");
        }

        public async Task<PackageItemDto?> GetPackageItemByIdAsync(int id)
        {
            return await _apiClient.GetAsync<PackageItemDto>($"PackageItem/{id}");
        }

        public async Task<PackageItemDto?> CreatePackageItemAsync(PackageItemDto packageItem)
        {
            return await _apiClient.PostAsync<PackageItemDto>("PackageItem", packageItem);
        }

        public async Task<PackageItemDto?> UpdatePackageItemAsync(int id, PackageItemDto packageItem)
        {
            return await _apiClient.PutAsync<PackageItemDto>($"PackageItem/{id}", packageItem);
        }

        public async Task<bool> DeletePackageItemAsync(int id)
        {
            return await _apiClient.DeleteAsync($"PackageItem/{id}");
        }

        public async Task<List<PackageItemDto>?> GetPackageItemsByPackageIdAsync(int packageId)
        {
            return await _apiClient.GetAsync<List<PackageItemDto>>($"PackageItem/package/{packageId}");
        }
    }
}
