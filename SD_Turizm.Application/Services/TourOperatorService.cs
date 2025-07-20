using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SD_Turizm.Core.Entities;
using SD_Turizm.Core.Interfaces;

namespace SD_Turizm.Application.Services
{
    public class TourOperatorService : ITourOperatorService
    {
        private readonly IUnitOfWork _unitOfWork;

        public TourOperatorService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<TourOperator>> GetAllTourOperatorsAsync()
        {
            return await _unitOfWork.Repository<TourOperator>().GetAllAsync();
        }

        public async Task<TourOperator> GetTourOperatorByIdAsync(int id)
        {
            return await _unitOfWork.Repository<TourOperator>().GetByIdAsync(id);
        }

        public async Task<TourOperator> GetTourOperatorByCodeAsync(string code)
        {
            var tourOperators = await _unitOfWork.Repository<TourOperator>().FindAsync(to => to.Code == code);
            return tourOperators.FirstOrDefault();
        }

        public async Task<TourOperator> CreateTourOperatorAsync(TourOperator tourOperator)
        {
            await _unitOfWork.Repository<TourOperator>().AddAsync(tourOperator);
            await _unitOfWork.SaveChangesAsync();
            return tourOperator;
        }

        public async Task UpdateTourOperatorAsync(TourOperator tourOperator)
        {
            await _unitOfWork.Repository<TourOperator>().UpdateAsync(tourOperator);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteTourOperatorAsync(int id)
        {
            await _unitOfWork.Repository<TourOperator>().DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<bool> TourOperatorExistsAsync(int id)
        {
            return await _unitOfWork.Repository<TourOperator>().ExistsAsync(id);
        }

        public async Task<bool> TourOperatorCodeExistsAsync(string code)
        {
            var tourOperators = await _unitOfWork.Repository<TourOperator>().FindAsync(to => to.Code == code);
            return tourOperators.Any();
        }
    }
} 