using SD_Turizm.Web.Models.DTOs;

namespace SD_Turizm.Web.Services
{
    public interface ISaleApiService
    {
        Task<List<SaleDto>> GetAllSalesAsync();
        Task<SaleDto?> GetSaleByIdAsync(int id);
        Task<SaleDto?> CreateSaleAsync(SaleDto sale);
        Task<SaleDto?> UpdateSaleAsync(int id, SaleDto sale);
        Task<bool> DeleteSaleAsync(int id);
    }
}
