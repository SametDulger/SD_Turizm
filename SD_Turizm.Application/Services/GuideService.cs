using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SD_Turizm.Core.Entities;
using SD_Turizm.Core.Interfaces;

namespace SD_Turizm.Application.Services
{
    public class GuideService : IGuideService
    {
        private readonly IUnitOfWork _unitOfWork;
        public GuideService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IEnumerable<Guide>> GetAllAsync()
        {
            return await _unitOfWork.Repository<Guide>().GetAllAsync();
        }
        public async Task<Guide> GetByIdAsync(int id)
        {
            return await _unitOfWork.Repository<Guide>().GetByIdAsync(id);
        }
        public async Task<Guide> CreateAsync(Guide entity)
        {
            await _unitOfWork.Repository<Guide>().AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            return entity;
        }
        public async Task UpdateAsync(Guide entity)
        {
            await _unitOfWork.Repository<Guide>().UpdateAsync(entity);
            await _unitOfWork.SaveChangesAsync();
        }
        public async Task DeleteAsync(int id)
        {
            await _unitOfWork.Repository<Guide>().DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
        }
        public async Task<bool> ExistsAsync(int id)
        {
            return await _unitOfWork.Repository<Guide>().ExistsAsync(id);
        }
    }
} 