using SD_Turizm.Core.Entities;
using SD_Turizm.Core.Interfaces;

namespace SD_Turizm.Application.Services
{
    public class SaleItemService : ISaleItemService
    {
        private readonly IUnitOfWork _unitOfWork;

        public SaleItemService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<SaleItem>> GetAllAsync()
        {
            return await _unitOfWork.Repository<SaleItem>().GetAllAsync();
        }

        public async Task<SaleItem?> GetByIdAsync(int id)
        {
            return await _unitOfWork.Repository<SaleItem>().GetByIdAsync(id);
        }

        public async Task<SaleItem> CreateAsync(SaleItem entity)
        {
            await _unitOfWork.Repository<SaleItem>().AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            return entity;
        }

        public async Task<SaleItem> UpdateAsync(SaleItem entity)
        {
            await _unitOfWork.Repository<SaleItem>().UpdateAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            return entity;
        }

        public async Task DeleteAsync(int id)
        {
            await _unitOfWork.Repository<SaleItem>().DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _unitOfWork.Repository<SaleItem>().ExistsAsync(id);
        }
    }
} 