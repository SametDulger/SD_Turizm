using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SD_Turizm.Core.Entities.Prices;
using SD_Turizm.Core.Interfaces;

namespace SD_Turizm.Application.Services
{
    public class CruisePriceService : ICruisePriceService
    {
        private readonly IUnitOfWork _unitOfWork;
        public CruisePriceService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IEnumerable<CruisePrice>> GetAllAsync()
        {
            return await _unitOfWork.Repository<CruisePrice>().GetAllAsync();
        }
        public async Task<CruisePrice> GetByIdAsync(int id)
        {
            return await _unitOfWork.Repository<CruisePrice>().GetByIdAsync(id);
        }
        public async Task<CruisePrice> CreateAsync(CruisePrice entity)
        {
            await _unitOfWork.Repository<CruisePrice>().AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            return entity;
        }
        public async Task UpdateAsync(CruisePrice entity)
        {
            await _unitOfWork.Repository<CruisePrice>().UpdateAsync(entity);
            await _unitOfWork.SaveChangesAsync();
        }
        public async Task DeleteAsync(int id)
        {
            await _unitOfWork.Repository<CruisePrice>().DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
        }
        public async Task<bool> ExistsAsync(int id)
        {
            return await _unitOfWork.Repository<CruisePrice>().ExistsAsync(id);
        }
    }
} 
