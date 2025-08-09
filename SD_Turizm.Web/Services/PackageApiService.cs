using SD_Turizm.Web.Models.DTOs;

namespace SD_Turizm.Web.Services
{
    public class PackageApiService : IPackageApiService
    {
        private readonly IApiClientService _apiClient;

        public PackageApiService(IApiClientService apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<List<PackageDto>?> GetAllPackagesAsync()
        {
            return await _apiClient.GetAsync<List<PackageDto>>("Package");
        }

        public async Task<PackageDto?> GetPackageByIdAsync(int id)
        {
            return await _apiClient.GetAsync<PackageDto>($"Package/{id}");
        }

        public async Task<PackageDto?> CreatePackageAsync(PackageDto package)
        {
            return await _apiClient.PostAsync<PackageDto>("Package", package);
        }

        public async Task<PackageDto?> UpdatePackageAsync(int id, PackageDto package)
        {
            return await _apiClient.PutAsync<PackageDto>($"Package/{id}", package);
        }

        public async Task<bool> DeletePackageAsync(int id)
        {
            return await _apiClient.DeleteAsync($"Package/{id}");
        }

        public async Task<List<PackageDto>?> GetActivePackagesAsync()
        {
            return await _apiClient.GetAsync<List<PackageDto>>("Package/active");
        }

        public async Task<List<PackageDto>?> GetPackagesByDestinationAsync(string destination)
        {
            return await _apiClient.GetAsync<List<PackageDto>>($"Package/destination/{destination}");
        }

        public async Task<List<PackageDto>?> GetPackagesByPriceRangeAsync(decimal minPrice, decimal maxPrice)
        {
            return await _apiClient.GetAsync<List<PackageDto>>($"Package/price-range?min={minPrice}&max={maxPrice}");
        }
    }
}
