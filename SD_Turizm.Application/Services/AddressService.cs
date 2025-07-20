using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SD_Turizm.Core.Entities;
using SD_Turizm.Core.Interfaces;

namespace SD_Turizm.Application.Services
{
    public class AddressService : IAddressService
    {
        private readonly IUnitOfWork _unitOfWork;
        public AddressService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IEnumerable<Address>> GetAllAsync()
        {
            return await _unitOfWork.Repository<Address>().GetAllAsync();
        }
        public async Task<Address> GetByIdAsync(int id)
        {
            return await _unitOfWork.Repository<Address>().GetByIdAsync(id);
        }
        public async Task<Address> CreateAsync(Address entity)
        {
            await _unitOfWork.Repository<Address>().AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            return entity;
        }
        public async Task UpdateAsync(Address entity)
        {
            await _unitOfWork.Repository<Address>().UpdateAsync(entity);
            await _unitOfWork.SaveChangesAsync();
        }
        public async Task DeleteAsync(int id)
        {
            await _unitOfWork.Repository<Address>().DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
        }
        public async Task<bool> ExistsAsync(int id)
        {
            return await _unitOfWork.Repository<Address>().ExistsAsync(id);
        }
    }
} 