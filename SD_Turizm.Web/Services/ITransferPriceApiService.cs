using SD_Turizm.Web.Models.DTOs;

namespace SD_Turizm.Web.Services
{
    public interface ITransferPriceApiService
    {
        Task<List<TransferPriceDto>?> GetAllTransferPricesAsync();
        Task<TransferPriceDto?> GetTransferPriceByIdAsync(int id);
        Task<TransferPriceDto?> CreateTransferPriceAsync(TransferPriceDto transferPrice);
        Task<TransferPriceDto?> UpdateTransferPriceAsync(int id, TransferPriceDto transferPrice);
        Task<bool> DeleteTransferPriceAsync(int id);
        Task<List<TransferPriceDto>?> GetTransferPricesByTransferCompanyIdAsync(int transferCompanyId);
    }
}
