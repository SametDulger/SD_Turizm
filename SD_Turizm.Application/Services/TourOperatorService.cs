using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SD_Turizm.Core.Entities;
using SD_Turizm.Core.Interfaces;
using SD_Turizm.Core.DTOs;

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

        public async Task<TourOperator?> GetTourOperatorByIdAsync(int id)
        {
            return await _unitOfWork.Repository<TourOperator>().GetByIdAsync(id);
        }

        public async Task<TourOperator?> GetTourOperatorByCodeAsync(string code)
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

        // V2 Methods
        public async Task<PagedResult<TourOperator>> GetTourOperatorsWithPaginationAsync(PaginationDto pagination, string? searchTerm = null, string? region = null, bool? isActive = null)
        {
            var tourOperators = await _unitOfWork.Repository<TourOperator>().GetAllAsync();
            
            // Apply filters
            if (!string.IsNullOrEmpty(searchTerm))
                tourOperators = tourOperators.Where(t => t.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase));
            
            // Region filter removed - TourOperator entity doesn't have Region property
            
            if (isActive.HasValue)
                tourOperators = tourOperators.Where(t => t.IsActive == isActive.Value);

            var totalCount = tourOperators.Count();
            var items = tourOperators.Skip((pagination.Page - 1) * pagination.PageSize).Take(pagination.PageSize).ToList();

            return new PagedResult<TourOperator>
            {
                Items = items,
                TotalCount = totalCount,
                Page = pagination.Page,
                PageSize = pagination.PageSize,
                TotalPages = (int)Math.Ceiling((double)totalCount / pagination.PageSize)
            };
        }

        public async Task<PagedResult<TourOperator>> SearchTourOperatorsAsync(PaginationDto pagination, string searchTerm, string? serviceType = null)
        {
            var tourOperators = await _unitOfWork.Repository<TourOperator>().GetAllAsync();
            
            tourOperators = tourOperators.Where(t => t.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase));
            
            // ServiceType filter removed - TourOperator entity doesn't have ServiceType property

            var totalCount = tourOperators.Count();
            var items = tourOperators.Skip((pagination.Page - 1) * pagination.PageSize).Take(pagination.PageSize).ToList();

            return new PagedResult<TourOperator>
            {
                Items = items,
                TotalCount = totalCount,
                Page = pagination.Page,
                PageSize = pagination.PageSize,
                TotalPages = (int)Math.Ceiling((double)totalCount / pagination.PageSize)
            };
        }

        public async Task<object> GetTourOperatorStatisticsAsync()
        {
            var tourOperators = await _unitOfWork.Repository<TourOperator>().GetAllAsync();
            
            return new
            {
                TotalTourOperators = tourOperators.Count(),
                ActiveTourOperators = tourOperators.Count(t => t.IsActive),
                // PopularRegions and PopularServiceTypes removed - TourOperator entity doesn't have these properties
            };
        }

        public async Task<int> BulkUpdateAsync(List<TourOperator> tourOperators)
        {
            foreach (var tourOperator in tourOperators)
            {
                await _unitOfWork.Repository<TourOperator>().UpdateAsync(tourOperator);
            }
            await _unitOfWork.SaveChangesAsync();
            return tourOperators.Count;
        }
    }
} 
