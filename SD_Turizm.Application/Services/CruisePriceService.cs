using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SD_Turizm.Core.Entities.Prices;
using SD_Turizm.Core.Interfaces;
using SD_Turizm.Core.DTOs;

namespace SD_Turizm.Application.Services
{
    public class CruisePriceService : ICruisePriceService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CruisePriceService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<CruisePrice>> GetAllAsync()
        {
            return await _unitOfWork.Repository<CruisePrice>().GetAllAsync();
        }

        public async Task<CruisePrice?> GetByIdAsync(int id)
        {
            return await _unitOfWork.Repository<CruisePrice>().GetByIdAsync(id);
        }

        public async Task<CruisePrice> CreateAsync(CruisePrice cruisePrice)
        {
            await _unitOfWork.Repository<CruisePrice>().AddAsync(cruisePrice);
            await _unitOfWork.SaveChangesAsync();
            return cruisePrice;
        }

        public async Task<CruisePrice> UpdateAsync(CruisePrice cruisePrice)
        {
            await _unitOfWork.Repository<CruisePrice>().UpdateAsync(cruisePrice);
            await _unitOfWork.SaveChangesAsync();
            return cruisePrice;
        }

        public async Task DeleteAsync(int id)
        {
            await _unitOfWork.Repository<CruisePrice>().DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _unitOfWork.Repository<CruisePrice>().ExistsAsync(id);
        }

        // V2 Methods
        public async Task<PagedResult<CruisePrice>> GetCruisePricesWithPaginationAsync(PaginationDto pagination, int? cruiseId = null, decimal? minPrice = null, decimal? maxPrice = null, DateTime? startDate = null, DateTime? endDate = null)
        {
            var prices = await _unitOfWork.Repository<CruisePrice>().GetAllAsync();
            
            // Apply filters
            if (cruiseId.HasValue)
                prices = prices.Where(p => p.CruiseId == cruiseId.Value);
            
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

            return new PagedResult<CruisePrice>
            {
                Items = items,
                TotalCount = totalCount,
                Page = pagination.Page,
                PageSize = pagination.PageSize,
                TotalPages = (int)Math.Ceiling((double)totalCount / pagination.PageSize)
            };
        }

        public async Task<IEnumerable<CruisePrice>> GetPricesByCruiseIdAsync(int cruiseId)
        {
            var prices = await _unitOfWork.Repository<CruisePrice>().GetAllAsync();
            return prices.Where(p => p.CruiseId == cruiseId).ToList();
        }

        public async Task<object> GetPriceStatisticsAsync()
        {
            var prices = await _unitOfWork.Repository<CruisePrice>().GetAllAsync();
            
            return new
            {
                TotalCount = prices.Count(),
                AveragePrice = prices.Any() ? prices.Average(p => p.AdultPrice) : 0,
                MinPrice = prices.Any() ? prices.Min(p => p.AdultPrice) : 0,
                MaxPrice = prices.Any() ? prices.Max(p => p.AdultPrice) : 0,
                TotalValue = prices.Sum(p => p.AdultPrice),
                CruiseDistribution = prices.GroupBy(p => p.CruiseId).Select(g => new { CruiseId = g.Key, Count = g.Count(), AveragePrice = g.Average(p => p.AdultPrice) })
            };
        }
    }
} 
