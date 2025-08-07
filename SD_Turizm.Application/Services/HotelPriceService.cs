using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SD_Turizm.Core.Entities.Prices;
using SD_Turizm.Core.Interfaces;

namespace SD_Turizm.Application.Services
{
    public class HotelPriceService : IHotelPriceService
    {
        private readonly IUnitOfWork _unitOfWork;
        public HotelPriceService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IEnumerable<HotelPrice>> GetAllAsync()
        {
            return await _unitOfWork.Repository<HotelPrice>().GetAllAsync();
        }
        public async Task<HotelPrice> GetByIdAsync(int id)
        {
            return await _unitOfWork.Repository<HotelPrice>().GetByIdAsync(id);
        }
        public async Task<HotelPrice> CreateAsync(HotelPrice entity)
        {
            await _unitOfWork.Repository<HotelPrice>().AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            return entity;
        }
        public async Task UpdateAsync(HotelPrice entity)
        {
            await _unitOfWork.Repository<HotelPrice>().UpdateAsync(entity);
            await _unitOfWork.SaveChangesAsync();
        }
        public async Task DeleteAsync(int id)
        {
            await _unitOfWork.Repository<HotelPrice>().DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
        }
        public async Task<bool> ExistsAsync(int id)
        {
            return await _unitOfWork.Repository<HotelPrice>().ExistsAsync(id);
        }
    }
} 
