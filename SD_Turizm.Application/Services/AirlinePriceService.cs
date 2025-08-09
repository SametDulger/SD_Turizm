using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SD_Turizm.Core.Entities.Prices;
using SD_Turizm.Core.Interfaces;
using SD_Turizm.Core.DTOs;

namespace SD_Turizm.Application.Services
{
    public class AirlinePriceService : IAirlinePriceService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AirlinePriceService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<AirlinePrice>> GetAllAsync()
        {
            return await _unitOfWork.Repository<AirlinePrice>().GetAllAsync();
        }

        public async Task<AirlinePrice?> GetByIdAsync(int id)
        {
            return await _unitOfWork.Repository<AirlinePrice>().GetByIdAsync(id);
        }

        public async Task<AirlinePrice> CreateAsync(AirlinePrice airlinePrice)
        {
            await _unitOfWork.Repository<AirlinePrice>().AddAsync(airlinePrice);
            await _unitOfWork.SaveChangesAsync();
            return airlinePrice;
        }

        public async Task<AirlinePrice> UpdateAsync(AirlinePrice airlinePrice)
        {
            await _unitOfWork.Repository<AirlinePrice>().UpdateAsync(airlinePrice);
            await _unitOfWork.SaveChangesAsync();
            return airlinePrice;
        }

        public async Task DeleteAsync(int id)
        {
            await _unitOfWork.Repository<AirlinePrice>().DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _unitOfWork.Repository<AirlinePrice>().ExistsAsync(id);
        }

        // V2 Methods
        public async Task<PagedResult<AirlinePrice>> GetAirlinePricesWithPaginationAsync(PaginationDto pagination, int? airlineId = null, decimal? minPrice = null, decimal? maxPrice = null, DateTime? startDate = null, DateTime? endDate = null)
        {
            var prices = await _unitOfWork.Repository<AirlinePrice>().GetAllAsync();
            
            // Apply filters
            if (airlineId.HasValue)
                prices = prices.Where(p => p.AirlineId == airlineId.Value);
            
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

            return new PagedResult<AirlinePrice>
            {
                Items = items,
                TotalCount = totalCount,
                Page = pagination.Page,
                PageSize = pagination.PageSize,
                TotalPages = (int)Math.Ceiling((double)totalCount / pagination.PageSize)
            };
        }

        public async Task<IEnumerable<AirlinePrice>> GetPricesByAirlineIdAsync(int airlineId)
        {
            var prices = await _unitOfWork.Repository<AirlinePrice>().GetAllAsync();
            return prices.Where(p => p.AirlineId == airlineId).ToList();
        }

        public async Task<object> GetPriceStatisticsAsync()
        {
            var prices = await _unitOfWork.Repository<AirlinePrice>().GetAllAsync();
            
            return new
            {
                TotalCount = prices.Count(),
                AveragePrice = prices.Any() ? prices.Average(p => p.AdultPrice) : 0,
                MinPrice = prices.Any() ? prices.Min(p => p.AdultPrice) : 0,
                MaxPrice = prices.Any() ? prices.Max(p => p.AdultPrice) : 0,
                TotalValue = prices.Sum(p => p.AdultPrice),
                AirlineDistribution = prices.GroupBy(p => p.AirlineId).Select(g => new { AirlineId = g.Key, Count = g.Count(), AveragePrice = g.Average(p => p.AdultPrice) })
            };
        }
    }
} 
