using SD_Turizm.Core.Entities;
using SD_Turizm.Core.Interfaces;

namespace SD_Turizm.Application.Services
{
    public class SalePersonService : ISalePersonService
    {
        private readonly IUnitOfWork _unitOfWork;

        public SalePersonService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<SalePerson>> GetAllAsync()
        {
            return await _unitOfWork.Repository<SalePerson>().GetAllAsync();
        }

        public async Task<SalePerson?> GetByIdAsync(int id)
        {
            return await _unitOfWork.Repository<SalePerson>().GetByIdAsync(id);
        }

        public async Task<SalePerson> CreateAsync(SalePerson entity)
        {
            await _unitOfWork.Repository<SalePerson>().AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            return entity;
        }

        public async Task<SalePerson> UpdateAsync(SalePerson entity)
        {
            await _unitOfWork.Repository<SalePerson>().UpdateAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            return entity;
        }

        public async Task DeleteAsync(int id)
        {
            await _unitOfWork.Repository<SalePerson>().DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _unitOfWork.Repository<SalePerson>().ExistsAsync(id);
        }
    }
} 
