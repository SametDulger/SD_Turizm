using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SD_Turizm.Core.Entities.Prices;
using SD_Turizm.Core.Interfaces;
using SD_Turizm.Core.DTOs;

namespace SD_Turizm.Application.Services
{
    public class GuidePriceService : IGuidePriceService
    {
        private readonly IUnitOfWork _unitOfWork;

        public GuidePriceService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<GuidePrice>> GetAllAsync()
        {
            return await _unitOfWork.Repository<GuidePrice>().GetAllAsync();
        }

        public async Task<GuidePrice?> GetByIdAsync(int id)
        {
            return await _unitOfWork.Repository<GuidePrice>().GetByIdAsync(id);
        }

        public async Task<GuidePrice> CreateAsync(GuidePrice guidePrice)
        {
            await _unitOfWork.Repository<GuidePrice>().AddAsync(guidePrice);
            await _unitOfWork.SaveChangesAsync();
            return guidePrice;
        }

        public async Task<GuidePrice> UpdateAsync(GuidePrice guidePrice)
        {
            await _unitOfWork.Repository<GuidePrice>().UpdateAsync(guidePrice);
            await _unitOfWork.SaveChangesAsync();
            return guidePrice;
        }

        public async Task DeleteAsync(int id)
        {
            await _unitOfWork.Repository<GuidePrice>().DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _unitOfWork.Repository<GuidePrice>().ExistsAsync(id);
        }

        // V2 Methods
        public async Task<PagedResult<GuidePrice>> GetGuidePricesWithPaginationAsync(PaginationDto pagination, int? guideId = null, decimal? minPrice = null, decimal? maxPrice = null, DateTime? startDate = null, DateTime? endDate = null)
        {
            var prices = await _unitOfWork.Repository<GuidePrice>().GetAllAsync();
            
            // Apply filters
            if (guideId.HasValue)
                prices = prices.Where(p => p.GuideId == guideId.Value);
            
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

            return new PagedResult<GuidePrice>
            {
                Items = items,
                TotalCount = totalCount,
                Page = pagination.Page,
                PageSize = pagination.PageSize,
                TotalPages = (int)Math.Ceiling((double)totalCount / pagination.PageSize)
            };
        }

        public async Task<IEnumerable<GuidePrice>> GetPricesByGuideIdAsync(int guideId)
        {
            var prices = await _unitOfWork.Repository<GuidePrice>().GetAllAsync();
            return prices.Where(p => p.GuideId == guideId).ToList();
        }

        public async Task<object> GetPriceStatisticsAsync()
        {
            var prices = await _unitOfWork.Repository<GuidePrice>().GetAllAsync();
            
            return new
            {
                TotalCount = prices.Count(),
                AveragePrice = prices.Any() ? prices.Average(p => p.AdultPrice) : 0,
                MinPrice = prices.Any() ? prices.Min(p => p.AdultPrice) : 0,
                MaxPrice = prices.Any() ? prices.Max(p => p.AdultPrice) : 0,
                TotalValue = prices.Sum(p => p.AdultPrice),
                GuideDistribution = prices.GroupBy(p => p.GuideId).Select(g => new { GuideId = g.Key, Count = g.Count(), AveragePrice = g.Average(p => p.AdultPrice) })
            };
        }
    }
} 
