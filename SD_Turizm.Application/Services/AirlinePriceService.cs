using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SD_Turizm.Core.Entities.Prices;
using SD_Turizm.Core.Interfaces;

namespace SD_Turizm.Application.Services
{
    public class AirlinePriceService : IAirlinePriceService
    {
        private readonly IUnitOfWork _unitOfWork;
        public AirlinePriceService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IEnumerable<AirlinePrice>> GetAllAsync()
        {
            return await _unitOfWork.Repository<AirlinePrice>().GetAllAsync();
        }
        public async Task<AirlinePrice> GetByIdAsync(int id)
        {
            return await _unitOfWork.Repository<AirlinePrice>().GetByIdAsync(id);
        }
        public async Task<AirlinePrice> CreateAsync(AirlinePrice entity)
        {
            await _unitOfWork.Repository<AirlinePrice>().AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            return entity;
        }
        public async Task UpdateAsync(AirlinePrice entity)
        {
            await _unitOfWork.Repository<AirlinePrice>().UpdateAsync(entity);
            await _unitOfWork.SaveChangesAsync();
        }
        public async Task DeleteAsync(int id)
        {
            await _unitOfWork.Repository<AirlinePrice>().DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
        }
        public async Task<bool> ExistsAsync(int id)
        {
            return await _unitOfWork.Repository<AirlinePrice>().ExistsAsync(id);
        }
    }
} 