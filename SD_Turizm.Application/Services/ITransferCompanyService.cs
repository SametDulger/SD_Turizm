using System.Collections.Generic;
using System.Threading.Tasks;
using SD_Turizm.Core.Entities;

namespace SD_Turizm.Application.Services
{
    public interface ITransferCompanyService
    {
        Task<IEnumerable<TransferCompany>> GetAllAsync();
        Task<TransferCompany> GetByIdAsync(int id);
        Task<TransferCompany> CreateAsync(TransferCompany entity);
        Task UpdateAsync(TransferCompany entity);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
} 