using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SD_Turizm.Core.Entities.Prices;
using SD_Turizm.Core.Interfaces;

namespace SD_Turizm.Application.Services
{
    public class GuidePriceService : IGuidePriceService
    {
        private readonly IUnitOfWork _unitOfWork;
        public GuidePriceService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IEnumerable<GuidePrice>> GetAllAsync()
        {
            return await _unitOfWork.Repository<GuidePrice>().GetAllAsync();
        }
        public async Task<GuidePrice> GetByIdAsync(int id)
        {
            return await _unitOfWork.Repository<GuidePrice>().GetByIdAsync(id);
        }
        public async Task<GuidePrice> CreateAsync(GuidePrice entity)
        {
            await _unitOfWork.Repository<GuidePrice>().AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            return entity;
        }
        public async Task UpdateAsync(GuidePrice entity)
        {
            await _unitOfWork.Repository<GuidePrice>().UpdateAsync(entity);
            await _unitOfWork.SaveChangesAsync();
        }
        public async Task DeleteAsync(int id)
        {
            await _unitOfWork.Repository<GuidePrice>().DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
        }
        public async Task<bool> ExistsAsync(int id)
        {
            return await _unitOfWork.Repository<GuidePrice>().ExistsAsync(id);
        }
    }
} 