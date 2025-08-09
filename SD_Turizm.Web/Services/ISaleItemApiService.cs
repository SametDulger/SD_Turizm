using SD_Turizm.Web.Models.DTOs;

namespace SD_Turizm.Web.Services
{
    public interface ISaleItemApiService
    {
        Task<List<SaleItemDto>?> GetAllSaleItemsAsync();
        Task<SaleItemDto?> GetSaleItemByIdAsync(int id);
        Task<SaleItemDto?> CreateSaleItemAsync(SaleItemDto saleItem);
        Task<SaleItemDto?> UpdateSaleItemAsync(int id, SaleItemDto saleItem);
        Task<bool> DeleteSaleItemAsync(int id);
        Task<List<SaleItemDto>?> GetSaleItemsBySaleIdAsync(int saleId);
    }
}
