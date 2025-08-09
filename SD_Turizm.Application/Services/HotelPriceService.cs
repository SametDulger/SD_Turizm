using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SD_Turizm.Core.Entities.Prices;
using SD_Turizm.Core.Interfaces;
using SD_Turizm.Core.DTOs;

namespace SD_Turizm.Application.Services
{
    public class HotelPriceService : IHotelPriceService
    {
        private readonly IUnitOfWork _unitOfWork;

        public HotelPriceService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<HotelPrice>> GetAllAsync()
        {
            return await _unitOfWork.Repository<HotelPrice>().GetAllAsync();
        }

        public async Task<HotelPrice?> GetByIdAsync(int id)
        {
            return await _unitOfWork.Repository<HotelPrice>().GetByIdAsync(id);
        }

        public async Task<HotelPrice> CreateAsync(HotelPrice hotelPrice)
        {
            await _unitOfWork.Repository<HotelPrice>().AddAsync(hotelPrice);
            await _unitOfWork.SaveChangesAsync();
            return hotelPrice;
        }

        public async Task<HotelPrice> UpdateAsync(HotelPrice hotelPrice)
        {
            await _unitOfWork.Repository<HotelPrice>().UpdateAsync(hotelPrice);
            await _unitOfWork.SaveChangesAsync();
            return hotelPrice;
        }

        public async Task DeleteAsync(int id)
        {
            await _unitOfWork.Repository<HotelPrice>().DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _unitOfWork.Repository<HotelPrice>().ExistsAsync(id);
        }

        // V2 Methods
        public async Task<PagedResult<HotelPrice>> GetHotelPricesWithPaginationAsync(PaginationDto pagination, int? hotelId = null, decimal? minPrice = null, decimal? maxPrice = null, DateTime? startDate = null, DateTime? endDate = null)
        {
            var prices = await _unitOfWork.Repository<HotelPrice>().GetAllAsync();
            
            // Apply filters
            if (hotelId.HasValue)
                prices = prices.Where(p => p.HotelId == hotelId.Value);
            
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

            return new PagedResult<HotelPrice>
            {
                Items = items,
                TotalCount = totalCount,
                Page = pagination.Page,
                PageSize = pagination.PageSize,
                TotalPages = (int)Math.Ceiling((double)totalCount / pagination.PageSize)
            };
        }

        public async Task<IEnumerable<HotelPrice>> GetPricesByHotelIdAsync(int hotelId)
        {
            var prices = await _unitOfWork.Repository<HotelPrice>().GetAllAsync();
            return prices.Where(p => p.HotelId == hotelId).ToList();
        }

        public async Task<object> GetPriceStatisticsAsync()
        {
            var prices = await _unitOfWork.Repository<HotelPrice>().GetAllAsync();
            
            return new
            {
                TotalCount = prices.Count(),
                AveragePrice = prices.Any() ? prices.Average(p => p.AdultPrice) : 0,
                MinPrice = prices.Any() ? prices.Min(p => p.AdultPrice) : 0,
                MaxPrice = prices.Any() ? prices.Max(p => p.AdultPrice) : 0,
                TotalValue = prices.Sum(p => p.AdultPrice),
                HotelDistribution = prices.GroupBy(p => p.HotelId).Select(g => new { HotelId = g.Key, Count = g.Count(), AveragePrice = g.Average(p => p.AdultPrice) })
            };
        }
    }
} 
