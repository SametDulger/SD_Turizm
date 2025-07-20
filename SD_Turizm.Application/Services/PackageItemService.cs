using SD_Turizm.Core.Entities;
using SD_Turizm.Core.Interfaces;

namespace SD_Turizm.Application.Services
{
    public class PackageItemService : IPackageItemService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PackageItemService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<PackageItem>> GetAllAsync()
        {
            return await _unitOfWork.Repository<PackageItem>().GetAllAsync();
        }

        public async Task<PackageItem?> GetByIdAsync(int id)
        {
            return await _unitOfWork.Repository<PackageItem>().GetByIdAsync(id);
        }

        public async Task<PackageItem> CreateAsync(PackageItem entity)
        {
            await _unitOfWork.Repository<PackageItem>().AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            return entity;
        }

        public async Task<PackageItem> UpdateAsync(PackageItem entity)
        {
            await _unitOfWork.Repository<PackageItem>().UpdateAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            return entity;
        }

        public async Task DeleteAsync(int id)
        {
            await _unitOfWork.Repository<PackageItem>().DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _unitOfWork.Repository<PackageItem>().ExistsAsync(id);
        }
    }
} 