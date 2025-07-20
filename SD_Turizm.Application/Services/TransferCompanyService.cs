using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SD_Turizm.Core.Entities;
using SD_Turizm.Core.Interfaces;

namespace SD_Turizm.Application.Services
{
    public class TransferCompanyService : ITransferCompanyService
    {
        private readonly IUnitOfWork _unitOfWork;
        public TransferCompanyService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IEnumerable<TransferCompany>> GetAllAsync()
        {
            return await _unitOfWork.Repository<TransferCompany>().GetAllAsync();
        }
        public async Task<TransferCompany> GetByIdAsync(int id)
        {
            return await _unitOfWork.Repository<TransferCompany>().GetByIdAsync(id);
        }
        public async Task<TransferCompany> CreateAsync(TransferCompany entity)
        {
            await _unitOfWork.Repository<TransferCompany>().AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            return entity;
        }
        public async Task UpdateAsync(TransferCompany entity)
        {
            await _unitOfWork.Repository<TransferCompany>().UpdateAsync(entity);
            await _unitOfWork.SaveChangesAsync();
        }
        public async Task DeleteAsync(int id)
        {
            await _unitOfWork.Repository<TransferCompany>().DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
        }
        public async Task<bool> ExistsAsync(int id)
        {
            return await _unitOfWork.Repository<TransferCompany>().ExistsAsync(id);
        }
    }
} 