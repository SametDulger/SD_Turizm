using SD_Turizm.Core.Entities;

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
    }
} 