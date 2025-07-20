using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SD_Turizm.Core.Entities;
using SD_Turizm.Core.Interfaces;

namespace SD_Turizm.Application.Services
{
    public class CruiseService : ICruiseService
    {
        private readonly IUnitOfWork _unitOfWork;
        public CruiseService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IEnumerable<Cruise>> GetAllAsync()
        {
            return await _unitOfWork.Repository<Cruise>().GetAllAsync();
        }
        public async Task<Cruise> GetByIdAsync(int id)
        {
            return await _unitOfWork.Repository<Cruise>().GetByIdAsync(id);
        }
        public async Task<Cruise> CreateAsync(Cruise entity)
        {
            await _unitOfWork.Repository<Cruise>().AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            return entity;
        }
        public async Task UpdateAsync(Cruise entity)
        {
            await _unitOfWork.Repository<Cruise>().UpdateAsync(entity);
            await _unitOfWork.SaveChangesAsync();
        }
        public async Task DeleteAsync(int id)
        {
            await _unitOfWork.Repository<Cruise>().DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
        }
        public async Task<bool> ExistsAsync(int id)
        {
            return await _unitOfWork.Repository<Cruise>().ExistsAsync(id);
        }
    }
} 