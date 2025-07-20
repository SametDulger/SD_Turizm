using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SD_Turizm.Core.Entities;
using SD_Turizm.Core.Interfaces;

namespace SD_Turizm.Application.Services
{
    public class ExchangeRateService : IExchangeRateService
    {
        private readonly IUnitOfWork _unitOfWork;
        public ExchangeRateService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IEnumerable<ExchangeRate>> GetAllAsync()
        {
            return await _unitOfWork.Repository<ExchangeRate>().GetAllAsync();
        }
        public async Task<ExchangeRate> GetByIdAsync(int id)
        {
            return await _unitOfWork.Repository<ExchangeRate>().GetByIdAsync(id);
        }
        public async Task<ExchangeRate> CreateAsync(ExchangeRate entity)
        {
            await _unitOfWork.Repository<ExchangeRate>().AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            return entity;
        }
        public async Task UpdateAsync(ExchangeRate entity)
        {
            await _unitOfWork.Repository<ExchangeRate>().UpdateAsync(entity);
            await _unitOfWork.SaveChangesAsync();
        }
        public async Task DeleteAsync(int id)
        {
            await _unitOfWork.Repository<ExchangeRate>().DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
        }
        public async Task<bool> ExistsAsync(int id)
        {
            return await _unitOfWork.Repository<ExchangeRate>().ExistsAsync(id);
        }
    }
} 