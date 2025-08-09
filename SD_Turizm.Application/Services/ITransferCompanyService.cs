using System.Collections.Generic;
using System.Threading.Tasks;
using SD_Turizm.Core.Entities;
using SD_Turizm.Core.DTOs;

namespace SD_Turizm.Application.Services
{
    public interface ITransferCompanyService
    {
        Task<IEnumerable<TransferCompany>> GetAllAsync();
        Task<TransferCompany?> GetByIdAsync(int id);
        Task<TransferCompany> CreateAsync(TransferCompany entity);
        Task UpdateAsync(TransferCompany entity);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        
        // V2 Methods
        Task<PagedResult<TransferCompany>> GetTransferCompaniesWithPaginationAsync(PaginationDto pagination, string? searchTerm = null, string? region = null, bool? isActive = null);
        Task<PagedResult<TransferCompany>> SearchTransferCompaniesAsync(PaginationDto pagination, string region, string? serviceType = null);
        Task<object> GetTransferCompanyStatisticsAsync();
    }
} 
