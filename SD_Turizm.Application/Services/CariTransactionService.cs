using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SD_Turizm.Core.Entities;
using SD_Turizm.Core.Interfaces;

namespace SD_Turizm.Application.Services
{
    public class CariTransactionService : ICariTransactionService
    {
        private readonly IUnitOfWork _unitOfWork;
        public CariTransactionService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IEnumerable<CariTransaction>> GetAllAsync()
        {
            return await _unitOfWork.Repository<CariTransaction>().GetAllAsync();
        }
        public async Task<CariTransaction> GetByIdAsync(int id)
        {
            return await _unitOfWork.Repository<CariTransaction>().GetByIdAsync(id);
        }
        public async Task<CariTransaction> CreateAsync(CariTransaction entity)
        {
            await _unitOfWork.Repository<CariTransaction>().AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            return entity;
        }
        public async Task UpdateAsync(CariTransaction entity)
        {
            await _unitOfWork.Repository<CariTransaction>().UpdateAsync(entity);
            await _unitOfWork.SaveChangesAsync();
        }
        public async Task DeleteAsync(int id)
        {
            await _unitOfWork.Repository<CariTransaction>().DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
        }
        public async Task<bool> ExistsAsync(int id)
        {
            return await _unitOfWork.Repository<CariTransaction>().ExistsAsync(id);
        }
    }
} 
