using System.Collections.Generic;
using System.Threading.Tasks;
using SD_Turizm.Core.Entities.Prices;

namespace SD_Turizm.Application.Services
{
    public interface ITransferPriceService
    {
        Task<IEnumerable<TransferPrice>> GetAllAsync();
        Task<TransferPrice> GetByIdAsync(int id);
        Task<TransferPrice> CreateAsync(TransferPrice entity);
        Task UpdateAsync(TransferPrice entity);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
} 