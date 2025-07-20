using System.Collections.Generic;
using System.Threading.Tasks;
using SD_Turizm.Core.Entities;

namespace SD_Turizm.Application.Services
{
    public interface IRentACarService
    {
        Task<IEnumerable<RentACar>> GetAllAsync();
        Task<RentACar> GetByIdAsync(int id);
        Task<RentACar> CreateAsync(RentACar entity);
        Task UpdateAsync(RentACar entity);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
} 