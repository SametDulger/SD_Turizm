using SD_Turizm.Web.Models.DTOs;

namespace SD_Turizm.Web.Services
{
    public interface IPackageItemApiService
    {
        Task<List<PackageItemDto>?> GetAllPackageItemsAsync();
        Task<PackageItemDto?> GetPackageItemByIdAsync(int id);
        Task<PackageItemDto?> CreatePackageItemAsync(PackageItemDto packageItem);
        Task<PackageItemDto?> UpdatePackageItemAsync(int id, PackageItemDto packageItem);
        Task<bool> DeletePackageItemAsync(int id);
        Task<List<PackageItemDto>?> GetPackageItemsByPackageIdAsync(int packageId);
    }
}
