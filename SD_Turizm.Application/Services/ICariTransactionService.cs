using System.Collections.Generic;
using System.Threading.Tasks;
using SD_Turizm.Core.Entities;

namespace SD_Turizm.Application.Services
{
    public interface ICariTransactionService
    {
        Task<IEnumerable<CariTransaction>> GetAllAsync();
        Task<CariTransaction> GetByIdAsync(int id);
        Task<CariTransaction> CreateAsync(CariTransaction entity);
        Task UpdateAsync(CariTransaction entity);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
} 
