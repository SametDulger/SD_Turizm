using SD_Turizm.Core.Entities;
using SD_Turizm.Core.Interfaces;

namespace SD_Turizm.Application.Services
{
    public class TourService : ITourService
    {
        private readonly IUnitOfWork _unitOfWork;

        public TourService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Tour>> GetAllAsync()
        {
            return await _unitOfWork.Repository<Tour>().GetAllAsync();
        }

        public async Task<Tour?> GetByIdAsync(int id)
        {
            return await _unitOfWork.Repository<Tour>().GetByIdAsync(id);
        }

        public async Task<Tour> CreateAsync(Tour entity)
        {
            await _unitOfWork.Repository<Tour>().AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            return entity;
        }

        public async Task<Tour> UpdateAsync(Tour entity)
        {
            await _unitOfWork.Repository<Tour>().UpdateAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            return entity;
        }

        public async Task DeleteAsync(int id)
        {
            await _unitOfWork.Repository<Tour>().DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _unitOfWork.Repository<Tour>().ExistsAsync(id);
        }
    }
} 
