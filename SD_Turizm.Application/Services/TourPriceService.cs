using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SD_Turizm.Core.Entities.Prices;
using SD_Turizm.Core.Interfaces;

namespace SD_Turizm.Application.Services
{
    public class TourPriceService : ITourPriceService
    {
        private readonly IUnitOfWork _unitOfWork;
        public TourPriceService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IEnumerable<TourPrice>> GetAllAsync()
        {
            return await _unitOfWork.Repository<TourPrice>().GetAllAsync();
        }
        public async Task<TourPrice> GetByIdAsync(int id)
        {
            return await _unitOfWork.Repository<TourPrice>().GetByIdAsync(id);
        }
        public async Task<TourPrice> CreateAsync(TourPrice entity)
        {
            await _unitOfWork.Repository<TourPrice>().AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            return entity;
        }
        public async Task UpdateAsync(TourPrice entity)
        {
            await _unitOfWork.Repository<TourPrice>().UpdateAsync(entity);
            await _unitOfWork.SaveChangesAsync();
        }
        public async Task DeleteAsync(int id)
        {
            await _unitOfWork.Repository<TourPrice>().DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
        }
        public async Task<bool> ExistsAsync(int id)
        {
            return await _unitOfWork.Repository<TourPrice>().ExistsAsync(id);
        }
    }
} 
