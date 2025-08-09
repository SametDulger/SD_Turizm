using System.Collections.Generic;
using System.Threading.Tasks;
using SD_Turizm.Core.Entities;
using SD_Turizm.Core.DTOs;

namespace SD_Turizm.Application.Services
{
    public interface ICariTransactionService
    {
        Task<IEnumerable<CariTransaction>> GetAllAsync();
        Task<CariTransaction?> GetByIdAsync(int id);
        Task<CariTransaction> CreateAsync(CariTransaction entity);
        Task UpdateAsync(CariTransaction entity);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        
        // V2 Methods
        Task<PagedResult<CariTransaction>> GetCariTransactionsWithPaginationAsync(PaginationDto pagination, string? cariCode = null, string? transactionType = null, DateTime? startDate = null, DateTime? endDate = null, decimal? minAmount = null, decimal? maxAmount = null);
        Task<PagedResult<CariTransaction>> GetCustomerTransactionsAsync(PaginationDto pagination, string cariCode);
        Task<decimal> GetCustomerBalanceAsync(string cariCode);
        Task<object> GetTransactionStatisticsAsync();
        Task<byte[]> ExportTransactionsAsync(string format = "excel", string? cariCode = null, DateTime? startDate = null, DateTime? endDate = null);
    }
} 
