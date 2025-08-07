using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SD_Turizm.Core.Entities;
using SD_Turizm.Core.Interfaces;

namespace SD_Turizm.Application.Services
{
    public class RentACarService : IRentACarService
    {
        private readonly IUnitOfWork _unitOfWork;
        public RentACarService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IEnumerable<RentACar>> GetAllAsync()
        {
            return await _unitOfWork.Repository<RentACar>().GetAllAsync();
        }
        public async Task<RentACar> GetByIdAsync(int id)
        {
            return await _unitOfWork.Repository<RentACar>().GetByIdAsync(id);
        }
        public async Task<RentACar> CreateAsync(RentACar entity)
        {
            await _unitOfWork.Repository<RentACar>().AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            return entity;
        }
        public async Task UpdateAsync(RentACar entity)
        {
            await _unitOfWork.Repository<RentACar>().UpdateAsync(entity);
            await _unitOfWork.SaveChangesAsync();
        }
        public async Task DeleteAsync(int id)
        {
            await _unitOfWork.Repository<RentACar>().DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
        }
        public async Task<bool> ExistsAsync(int id)
        {
            return await _unitOfWork.Repository<RentACar>().ExistsAsync(id);
        }
    }
} 
