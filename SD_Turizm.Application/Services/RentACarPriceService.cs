using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SD_Turizm.Core.Entities.Prices;
using SD_Turizm.Core.Interfaces;
using SD_Turizm.Core.DTOs;

namespace SD_Turizm.Application.Services
{
    public class RentACarPriceService : IRentACarPriceService
    {
        private readonly IUnitOfWork _unitOfWork;

        public RentACarPriceService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<RentACarPrice>> GetAllAsync()
        {
            return await _unitOfWork.Repository<RentACarPrice>().GetAllAsync();
        }

        public async Task<RentACarPrice?> GetByIdAsync(int id)
        {
            return await _unitOfWork.Repository<RentACarPrice>().GetByIdAsync(id);
        }

        public async Task<RentACarPrice> CreateAsync(RentACarPrice rentACarPrice)
        {
            await _unitOfWork.Repository<RentACarPrice>().AddAsync(rentACarPrice);
            await _unitOfWork.SaveChangesAsync();
            return rentACarPrice;
        }

        public async Task<RentACarPrice> UpdateAsync(RentACarPrice rentACarPrice)
        {
            await _unitOfWork.Repository<RentACarPrice>().UpdateAsync(rentACarPrice);
            await _unitOfWork.SaveChangesAsync();
            return rentACarPrice;
        }

        public async Task DeleteAsync(int id)
        {
            await _unitOfWork.Repository<RentACarPrice>().DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _unitOfWork.Repository<RentACarPrice>().ExistsAsync(id);
        }

        // V2 Methods
        public async Task<PagedResult<RentACarPrice>> GetRentACarPricesWithPaginationAsync(PaginationDto pagination, int? rentACarId = null, decimal? minPrice = null, decimal? maxPrice = null, DateTime? startDate = null, DateTime? endDate = null)
        {
            var prices = await _unitOfWork.Repository<RentACarPrice>().GetAllAsync();
            
            // Apply filters
            if (rentACarId.HasValue)
                prices = prices.Where(p => p.RentACarId == rentACarId.Value);
            
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

            return new PagedResult<RentACarPrice>
            {
                Items = items,
                TotalCount = totalCount,
                Page = pagination.Page,
                PageSize = pagination.PageSize,
                TotalPages = (int)Math.Ceiling((double)totalCount / pagination.PageSize)
            };
        }

        public async Task<IEnumerable<RentACarPrice>> GetPricesByRentACarIdAsync(int rentACarId)
        {
            var prices = await _unitOfWork.Repository<RentACarPrice>().GetAllAsync();
            return prices.Where(p => p.RentACarId == rentACarId).ToList();
        }

        public async Task<object> GetPriceStatisticsAsync()
        {
            var prices = await _unitOfWork.Repository<RentACarPrice>().GetAllAsync();
            
            return new
            {
                TotalCount = prices.Count(),
                AveragePrice = prices.Any() ? prices.Average(p => p.AdultPrice) : 0,
                MinPrice = prices.Any() ? prices.Min(p => p.AdultPrice) : 0,
                MaxPrice = prices.Any() ? prices.Max(p => p.AdultPrice) : 0,
                TotalValue = prices.Sum(p => p.AdultPrice),
                RentACarDistribution = prices.GroupBy(p => p.RentACarId).Select(g => new { RentACarId = g.Key, Count = g.Count(), AveragePrice = g.Average(p => p.AdultPrice) })
            };
        }
    }
} 
