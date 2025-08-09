using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SD_Turizm.Core.Entities;
using SD_Turizm.Core.Interfaces;

namespace SD_Turizm.Application.Services
{
    public class AirlineService : IAirlineService
    {
        private readonly IUnitOfWork _unitOfWork;
        public AirlineService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IEnumerable<Airline>> GetAllAsync()
        {
            return await _unitOfWork.Repository<Airline>().GetAllAsync();
        }
        public async Task<Airline?> GetByIdAsync(int id)
        {
            return await _unitOfWork.Repository<Airline>().GetByIdAsync(id);
        }
        public async Task<Airline> CreateAsync(Airline entity)
        {
            await _unitOfWork.Repository<Airline>().AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            return entity;
        }
        public async Task UpdateAsync(Airline entity)
        {
            await _unitOfWork.Repository<Airline>().UpdateAsync(entity);
            await _unitOfWork.SaveChangesAsync();
        }
        public async Task DeleteAsync(int id)
        {
            await _unitOfWork.Repository<Airline>().DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
        }
        public async Task<bool> ExistsAsync(int id)
        {
            return await _unitOfWork.Repository<Airline>().ExistsAsync(id);
        }
    }
} 
