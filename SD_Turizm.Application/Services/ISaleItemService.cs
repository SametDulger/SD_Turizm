using SD_Turizm.Core.Entities;
using SD_Turizm.Core.DTOs;

namespace SD_Turizm.Application.Services
{
    public interface ISaleItemService
    {
        Task<IEnumerable<SaleItem>> GetAllAsync();
        Task<SaleItem?> GetByIdAsync(int id);
        Task<SaleItem> CreateAsync(SaleItem entity);
        Task<SaleItem> UpdateAsync(SaleItem entity);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        
        // V2 Methods
        Task<PagedResult<SaleItem>> GetSaleItemsWithPaginationAsync(PaginationDto pagination, int? saleId = null, string? productType = null, decimal? minAmount = null, decimal? maxAmount = null);
        Task<IEnumerable<SaleItem>> GetItemsBySaleIdAsync(int saleId);
        Task<object> GetSaleItemStatisticsAsync(int? saleId = null, string? productType = null);
    }
} 
