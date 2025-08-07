using System.Collections.Generic;
using System.Threading.Tasks;
using SD_Turizm.Core.Entities;

namespace SD_Turizm.Application.Services
{
    public interface ICruiseService
    {
        Task<IEnumerable<Cruise>> GetAllAsync();
        Task<Cruise> GetByIdAsync(int id);
        Task<Cruise> CreateAsync(Cruise entity);
        Task UpdateAsync(Cruise entity);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
} 
