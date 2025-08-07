using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SD_Turizm.Core.Entities;
using SD_Turizm.Core.Interfaces;

namespace SD_Turizm.Application.Services
{
    public class PackageService : IPackageService
    {
        private readonly IUnitOfWork _unitOfWork;
        public PackageService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IEnumerable<Package>> GetAllAsync()
        {
            return await _unitOfWork.Repository<Package>().GetAllAsync();
        }
        public async Task<Package> GetByIdAsync(int id)
        {
            return await _unitOfWork.Repository<Package>().GetByIdAsync(id);
        }
        public async Task<Package> CreateAsync(Package entity)
        {
            await _unitOfWork.Repository<Package>().AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            return entity;
        }
        public async Task UpdateAsync(Package entity)
        {
            await _unitOfWork.Repository<Package>().UpdateAsync(entity);
            await _unitOfWork.SaveChangesAsync();
        }
        public async Task DeleteAsync(int id)
        {
            await _unitOfWork.Repository<Package>().DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
        }
        public async Task<bool> ExistsAsync(int id)
        {
            return await _unitOfWork.Repository<Package>().ExistsAsync(id);
        }
    }
} 
