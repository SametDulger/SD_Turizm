using SD_Turizm.Web.Models.DTOs;

namespace SD_Turizm.Web.Services
{
    public interface IVendorApiService
    {
        Task<List<VendorDto>> GetAllVendorsAsync();
        Task<List<VendorDto>> GetActiveVendorsAsync();
        Task<VendorDto?> GetVendorByIdAsync(int id);
        Task<VendorDto?> CreateVendorAsync(VendorDto vendor);
        Task<VendorDto?> UpdateVendorAsync(int id, VendorDto vendor);
        Task<bool> DeleteVendorAsync(int id);
    }
}
