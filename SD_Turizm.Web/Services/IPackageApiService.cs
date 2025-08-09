using SD_Turizm.Web.Models.DTOs;

namespace SD_Turizm.Web.Services
{
    public interface IPackageApiService
    {
        Task<List<PackageDto>?> GetAllPackagesAsync();
        Task<PackageDto?> GetPackageByIdAsync(int id);
        Task<PackageDto?> CreatePackageAsync(PackageDto package);
        Task<PackageDto?> UpdatePackageAsync(int id, PackageDto package);
        Task<bool> DeletePackageAsync(int id);
        Task<List<PackageDto>?> GetActivePackagesAsync();
        Task<List<PackageDto>?> GetPackagesByDestinationAsync(string destination);
        Task<List<PackageDto>?> GetPackagesByPriceRangeAsync(decimal minPrice, decimal maxPrice);
    }
}
