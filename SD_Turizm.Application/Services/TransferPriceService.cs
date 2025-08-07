using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SD_Turizm.Core.Entities.Prices;
using SD_Turizm.Core.Interfaces;

namespace SD_Turizm.Application.Services
{
    public class TransferPriceService : ITransferPriceService
    {
        private readonly IUnitOfWork _unitOfWork;
        public TransferPriceService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IEnumerable<TransferPrice>> GetAllAsync()
        {
            return await _unitOfWork.Repository<TransferPrice>().GetAllAsync();
        }
        public async Task<TransferPrice> GetByIdAsync(int id)
        {
            return await _unitOfWork.Repository<TransferPrice>().GetByIdAsync(id);
        }
        public async Task<TransferPrice> CreateAsync(TransferPrice entity)
        {
            await _unitOfWork.Repository<TransferPrice>().AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            return entity;
        }
        public async Task UpdateAsync(TransferPrice entity)
        {
            await _unitOfWork.Repository<TransferPrice>().UpdateAsync(entity);
            await _unitOfWork.SaveChangesAsync();
        }
        public async Task DeleteAsync(int id)
        {
            await _unitOfWork.Repository<TransferPrice>().DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
        }
        public async Task<bool> ExistsAsync(int id)
        {
            return await _unitOfWork.Repository<TransferPrice>().ExistsAsync(id);
        }
    }
} 
