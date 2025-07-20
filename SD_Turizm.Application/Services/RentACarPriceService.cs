using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SD_Turizm.Core.Entities.Prices;
using SD_Turizm.Core.Interfaces;

namespace SD_Turizm.Application.Services
{
    public class RentACarPriceService : IRentACarPriceService
    {
        private readonly IUnitOfWork _unitOfWork;
        public RentACarPriceService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IEnumerable<RentACarPrice>> GetAllAsync()
        {
            return await _unitOfWork.Repository<RentACarPrice>().GetAllAsync();
        }
        public async Task<RentACarPrice> GetByIdAsync(int id)
        {
            return await _unitOfWork.Repository<RentACarPrice>().GetByIdAsync(id);
        }
        public async Task<RentACarPrice> CreateAsync(RentACarPrice entity)
        {
            await _unitOfWork.Repository<RentACarPrice>().AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            return entity;
        }
        public async Task UpdateAsync(RentACarPrice entity)
        {
            await _unitOfWork.Repository<RentACarPrice>().UpdateAsync(entity);
            await _unitOfWork.SaveChangesAsync();
        }
        public async Task DeleteAsync(int id)
        {
            await _unitOfWork.Repository<RentACarPrice>().DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
        }
        public async Task<bool> ExistsAsync(int id)
        {
            return await _unitOfWork.Repository<RentACarPrice>().ExistsAsync(id);
        }
    }
} 