using System.Collections.Generic;
using System.Threading.Tasks;
using SD_Turizm.Core.Entities;
using SD_Turizm.Core.DTOs;

namespace SD_Turizm.Application.Services
{
    public interface ISalePersonService
    {
        Task<IEnumerable<SalePerson>> GetAllAsync();
        Task<SalePerson?> GetByIdAsync(int id);
        Task<SalePerson> CreateAsync(SalePerson salePerson);
        Task<SalePerson> UpdateAsync(SalePerson salePerson);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        
        // V2 Methods
        Task<PagedResult<SalePerson>> GetSalePersonsWithPaginationAsync(PaginationDto pagination, string? searchTerm = null, string? region = null, bool? isActive = null);
        Task<PagedResult<SalePerson>> SearchSalePersonsAsync(PaginationDto pagination, string region, string? serviceType = null);
        Task<object> GetSalePersonStatisticsAsync();
    }
} 
