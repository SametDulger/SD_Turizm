using SD_Turizm.Web.Models.DTOs;

namespace SD_Turizm.Web.Services
{
    public interface ITransferCompanyApiService
    {
        Task<List<TransferCompanyDto>> GetAllTransferCompaniesAsync();
        Task<TransferCompanyDto?> GetTransferCompanyByIdAsync(int id);
        Task<TransferCompanyDto?> CreateTransferCompanyAsync(TransferCompanyDto transferCompany);
        Task<TransferCompanyDto?> UpdateTransferCompanyAsync(int id, TransferCompanyDto transferCompany);
        Task<bool> DeleteTransferCompanyAsync(int id);
    }
}
