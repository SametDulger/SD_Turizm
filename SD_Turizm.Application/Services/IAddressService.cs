using System.Collections.Generic;
using System.Threading.Tasks;
using SD_Turizm.Core.Entities;
using SD_Turizm.Core.DTOs;

namespace SD_Turizm.Application.Services
{
    public interface IAddressService
    {
        Task<IEnumerable<Address>> GetAllAsync();
        Task<Address?> GetByIdAsync(int id);
        Task<Address> CreateAsync(Address address);
        Task<Address> UpdateAsync(Address address);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        
        // V2 Methods
        Task<PagedResult<Address>> GetAddressesWithPaginationAsync(PaginationDto pagination, string? searchTerm = null, string? city = null, string? country = null);
        Task<PagedResult<Address>> SearchAddressesAsync(PaginationDto pagination, string city, string? country = null);
        Task<object> GetAddressStatisticsAsync();
        Task<List<Address>> BulkUpdateAsync(List<Address> addresses);
    }
} 
