using System.Collections.Generic;
using System.Threading.Tasks;
using SD_Turizm.Core.Entities;

namespace SD_Turizm.Application.Services
{
    public interface IAddressService
    {
        Task<IEnumerable<Address>> GetAllAsync();
        Task<Address> GetByIdAsync(int id);
        Task<Address> CreateAsync(Address entity);
        Task UpdateAsync(Address entity);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
} 