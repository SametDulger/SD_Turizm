using System.Collections.Generic;
using System.Threading.Tasks;
using SD_Turizm.Core.Entities;

namespace SD_Turizm.Application.Services
{
    public interface IAirlineService
    {
        Task<IEnumerable<Airline>> GetAllAsync();
        Task<Airline> GetByIdAsync(int id);
        Task<Airline> CreateAsync(Airline entity);
        Task UpdateAsync(Airline entity);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
} 
