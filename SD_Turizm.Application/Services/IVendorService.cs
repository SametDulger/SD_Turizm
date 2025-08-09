using System.Collections.Generic;
using System.Threading.Tasks;
using SD_Turizm.Core.Entities;
using SD_Turizm.Core.DTOs;

namespace SD_Turizm.Application.Services
{
    public interface IVendorService
    {
        Task<IEnumerable<VendorDto>> GetAllVendorsAsync();
        Task<VendorDto?> GetVendorByIdAsync(int id);
        Task<IEnumerable<VendorDto>> GetActiveVendorsAsync();
        Task<IEnumerable<VendorDto>> GetVendorsByTypeAsync(string vendorType);
        Task<PagedResult<VendorDto>> GetVendorsWithPaginationAsync(int page, int pageSize, string? searchTerm = null, string? vendorType = null, bool? isActive = null);
        Task<object> GetVendorStatisticsAsync();
        Task<VendorDto> CreateVendorAsync(VendorDto vendorDto);
        Task<VendorDto?> UpdateVendorAsync(VendorDto vendorDto);
        Task<bool> DeleteVendorAsync(int id);
        Task<VendorDto?> ToggleVendorStatusAsync(int id);
    }
}
