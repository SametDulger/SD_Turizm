using System.Collections.Generic;
using System.Threading.Tasks;
using SD_Turizm.Core.Entities.Prices;
using SD_Turizm.Core.DTOs;

namespace SD_Turizm.Application.Services
{
    public interface ITransferPriceService
    {
        Task<IEnumerable<TransferPrice>> GetAllAsync();
        Task<TransferPrice?> GetByIdAsync(int id);
        Task<TransferPrice> CreateAsync(TransferPrice entity);
        Task UpdateAsync(TransferPrice entity);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        
        // V2 Methods
        Task<PagedResult<TransferPrice>> GetTransferPricesWithPaginationAsync(PaginationDto pagination, int? transferCompanyId = null, decimal? minPrice = null, decimal? maxPrice = null, DateTime? startDate = null, DateTime? endDate = null);
        Task<IEnumerable<TransferPrice>> GetPricesByTransferCompanyIdAsync(int transferCompanyId);
        Task<object> GetPriceStatisticsAsync(int? transferCompanyId = null, DateTime? startDate = null, DateTime? endDate = null);
    }
} 
