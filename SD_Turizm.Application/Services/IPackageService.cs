using System.Collections.Generic;
using System.Threading.Tasks;
using SD_Turizm.Core.Entities;

namespace SD_Turizm.Application.Services
{
    public interface IPackageService
    {
        Task<IEnumerable<Package>> GetAllAsync();
        Task<Package> GetByIdAsync(int id);
        Task<Package> CreateAsync(Package entity);
        Task UpdateAsync(Package entity);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
} 
