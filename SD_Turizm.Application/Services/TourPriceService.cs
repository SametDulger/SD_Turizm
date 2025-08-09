using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SD_Turizm.Core.Entities.Prices;
using SD_Turizm.Core.Interfaces;
using SD_Turizm.Core.DTOs;

namespace SD_Turizm.Application.Services
{
    public class TourPriceService : ITourPriceService
    {
        private readonly IUnitOfWork _unitOfWork;
        public TourPriceService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IEnumerable<TourPrice>> GetAllAsync()
        {
            return await _unitOfWork.Repository<TourPrice>().GetAllAsync();
        }
        public async Task<TourPrice?> GetByIdAsync(int id)
        {
            return await _unitOfWork.Repository<TourPrice>().GetByIdAsync(id);
        }
        public async Task<TourPrice> CreateAsync(TourPrice entity)
        {
            await _unitOfWork.Repository<TourPrice>().AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            return entity;
        }
        public async Task UpdateAsync(TourPrice entity)
        {
            await _unitOfWork.Repository<TourPrice>().UpdateAsync(entity);
            await _unitOfWork.SaveChangesAsync();
        }
        public async Task DeleteAsync(int id)
        {
            await _unitOfWork.Repository<TourPrice>().DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
        }
        public async Task<bool> ExistsAsync(int id)
        {
            return await _unitOfWork.Repository<TourPrice>().ExistsAsync(id);
        }

        // V2 Methods
        public async Task<PagedResult<TourPrice>> GetTourPricesWithPaginationAsync(PaginationDto pagination, int? tourId = null, decimal? minPrice = null, decimal? maxPrice = null, DateTime? startDate = null, DateTime? endDate = null)
        {
            var prices = await _unitOfWork.Repository<TourPrice>().GetAllAsync();
            
            // Apply filters
            if (tourId.HasValue)
                prices = prices.Where(p => p.TourId == tourId.Value);
            
            if (minPrice.HasValue)
                prices = prices.Where(p => p.AdultPrice >= minPrice.Value);
            
            if (maxPrice.HasValue)
                prices = prices.Where(p => p.AdultPrice <= maxPrice.Value);
            
            if (startDate.HasValue)
                prices = prices.Where(p => p.StartDate >= startDate.Value);
            
            if (endDate.HasValue)
                prices = prices.Where(p => p.EndDate <= endDate.Value);

            var totalCount = prices.Count();
            var items = prices.Skip((pagination.Page - 1) * pagination.PageSize).Take(pagination.PageSize).ToList();

            return new PagedResult<TourPrice>
            {
                Items = items,
                TotalCount = totalCount,
                Page = pagination.Page,
                PageSize = pagination.PageSize,
                TotalPages = (int)Math.Ceiling((double)totalCount / pagination.PageSize)
            };
        }

        public async Task<IEnumerable<TourPrice>> GetPricesByTourIdAsync(int tourId)
        {
            var prices = await _unitOfWork.Repository<TourPrice>().GetAllAsync();
            return prices.Where(p => p.TourId == tourId);
        }

        public async Task<object> GetPriceStatisticsAsync(int? tourId = null, DateTime? startDate = null, DateTime? endDate = null)
        {
            var prices = await _unitOfWork.Repository<TourPrice>().GetAllAsync();
            
            if (tourId.HasValue)
                prices = prices.Where(p => p.TourId == tourId.Value);
            
            if (startDate.HasValue)
                prices = prices.Where(p => p.StartDate >= startDate.Value);
            
            if (endDate.HasValue)
                prices = prices.Where(p => p.EndDate <= endDate.Value);
            
            return new
            {
                TotalPrices = prices.Count(),
                AveragePrice = prices.Any() ? prices.Average(p => p.AdultPrice) : 0,
                MinPrice = prices.Any() ? prices.Min(p => p.AdultPrice) : 0,
                MaxPrice = prices.Any() ? prices.Max(p => p.AdultPrice) : 0,
                PriceRange = prices.Any() ? prices.Max(p => p.AdultPrice) - prices.Min(p => p.AdultPrice) : 0
            };
        }
    }
} 
