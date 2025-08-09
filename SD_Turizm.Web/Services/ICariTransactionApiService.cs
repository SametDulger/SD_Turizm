using SD_Turizm.Web.Models.DTOs;

namespace SD_Turizm.Web.Services
{
    public interface ICariTransactionApiService
    {
        Task<List<CariTransactionDto>?> GetAllTransactionsAsync();
        Task<CariTransactionDto?> GetTransactionByIdAsync(int id);
        Task<CariTransactionDto?> CreateTransactionAsync(CariTransactionDto transaction);
        Task<CariTransactionDto?> UpdateTransactionAsync(int id, CariTransactionDto transaction);
        Task<bool> DeleteTransactionAsync(int id);
        Task<List<CariTransactionDto>?> GetTransactionsByVendorAsync(int vendorId);
        Task<List<CariTransactionDto>?> GetTransactionsByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<decimal> GetVendorBalanceAsync(int vendorId);
    }
}
