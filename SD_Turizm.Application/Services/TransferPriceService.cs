using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SD_Turizm.Core.Entities.Prices;
using SD_Turizm.Core.Interfaces;
using SD_Turizm.Core.DTOs;

namespace SD_Turizm.Application.Services
{
    public class TransferPriceService : ITransferPriceService
    {
        private readonly IUnitOfWork _unitOfWork;
        public TransferPriceService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IEnumerable<TransferPrice>> GetAllAsync()
        {
            return await _unitOfWork.Repository<TransferPrice>().GetAllAsync();
        }
        public async Task<TransferPrice?> GetByIdAsync(int id)
        {
            return await _unitOfWork.Repository<TransferPrice>().GetByIdAsync(id);
        }
        public async Task<TransferPrice> CreateAsync(TransferPrice entity)
        {
            await _unitOfWork.Repository<TransferPrice>().AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            return entity;
        }
        public async Task UpdateAsync(TransferPrice entity)
        {
            await _unitOfWork.Repository<TransferPrice>().UpdateAsync(entity);
            await _unitOfWork.SaveChangesAsync();
        }
        public async Task DeleteAsync(int id)
        {
            await _unitOfWork.Repository<TransferPrice>().DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
        }
        public async Task<bool> ExistsAsync(int id)
        {
            return await _unitOfWork.Repository<TransferPrice>().ExistsAsync(id);
        }

        // V2 Methods
        public async Task<PagedResult<TransferPrice>> GetTransferPricesWithPaginationAsync(PaginationDto pagination, int? transferCompanyId = null, decimal? minPrice = null, decimal? maxPrice = null, DateTime? startDate = null, DateTime? endDate = null)
        {
            var prices = await _unitOfWork.Repository<TransferPrice>().GetAllAsync();
            
            // Apply filters
            if (transferCompanyId.HasValue)
                prices = prices.Where(p => p.TransferCompanyId == transferCompanyId.Value);
            
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

            return new PagedResult<TransferPrice>
            {
                Items = items,
                TotalCount = totalCount,
                Page = pagination.Page,
                PageSize = pagination.PageSize,
                TotalPages = (int)Math.Ceiling((double)totalCount / pagination.PageSize)
            };
        }

        public async Task<IEnumerable<TransferPrice>> GetPricesByTransferCompanyIdAsync(int transferCompanyId)
        {
            var prices = await _unitOfWork.Repository<TransferPrice>().GetAllAsync();
            return prices.Where(p => p.TransferCompanyId == transferCompanyId);
        }

        public async Task<object> GetPriceStatisticsAsync(int? transferCompanyId = null, DateTime? startDate = null, DateTime? endDate = null)
        {
            var prices = await _unitOfWork.Repository<TransferPrice>().GetAllAsync();
            
            if (transferCompanyId.HasValue)
                prices = prices.Where(p => p.TransferCompanyId == transferCompanyId.Value);
            
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
