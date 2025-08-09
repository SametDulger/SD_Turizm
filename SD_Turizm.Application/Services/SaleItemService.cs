using SD_Turizm.Core.Entities;
using SD_Turizm.Core.Interfaces;
using SD_Turizm.Core.DTOs;

namespace SD_Turizm.Application.Services
{
    public class SaleItemService : ISaleItemService
    {
        private readonly IUnitOfWork _unitOfWork;

        public SaleItemService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<SaleItem>> GetAllAsync()
        {
            return await _unitOfWork.Repository<SaleItem>().GetAllAsync();
        }

        public async Task<SaleItem?> GetByIdAsync(int id)
        {
            return await _unitOfWork.Repository<SaleItem>().GetByIdAsync(id);
        }

        public async Task<SaleItem> CreateAsync(SaleItem entity)
        {
            await _unitOfWork.Repository<SaleItem>().AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            return entity;
        }

        public async Task<SaleItem> UpdateAsync(SaleItem entity)
        {
            await _unitOfWork.Repository<SaleItem>().UpdateAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            return entity;
        }

        public async Task DeleteAsync(int id)
        {
            await _unitOfWork.Repository<SaleItem>().DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _unitOfWork.Repository<SaleItem>().ExistsAsync(id);
        }

        // V2 Methods
        public async Task<PagedResult<SaleItem>> GetSaleItemsWithPaginationAsync(PaginationDto pagination, int? saleId = null, string? productType = null, decimal? minAmount = null, decimal? maxAmount = null)
        {
            var items = await _unitOfWork.Repository<SaleItem>().GetAllAsync();
            
            // Apply filters
            if (saleId.HasValue)
                items = items.Where(i => i.SaleId == saleId.Value);
            
            // ProductType filter removed - SaleItem entity doesn't have ProductType property
            
            if (minAmount.HasValue)
                items = items.Where(i => i.TotalPrice >= minAmount.Value);
            
            if (maxAmount.HasValue)
                items = items.Where(i => i.TotalPrice <= maxAmount.Value);

            var totalCount = items.Count();
            var result = items.Skip((pagination.Page - 1) * pagination.PageSize).Take(pagination.PageSize).ToList();

            return new PagedResult<SaleItem>
            {
                Items = result,
                TotalCount = totalCount,
                Page = pagination.Page,
                PageSize = pagination.PageSize,
                TotalPages = (int)Math.Ceiling((double)totalCount / pagination.PageSize)
            };
        }

        public async Task<IEnumerable<SaleItem>> GetItemsBySaleIdAsync(int saleId)
        {
            var items = await _unitOfWork.Repository<SaleItem>().GetAllAsync();
            return items.Where(i => i.SaleId == saleId);
        }

        public async Task<object> GetSaleItemStatisticsAsync(int? saleId = null, string? productType = null)
        {
            var items = await _unitOfWork.Repository<SaleItem>().GetAllAsync();
            
            if (saleId.HasValue)
                items = items.Where(i => i.SaleId == saleId.Value);
            
            // ProductType filter removed - SaleItem entity doesn't have ProductType property
            
            return new
            {
                TotalItems = items.Count(),
                TotalAmount = items.Sum(i => i.TotalPrice),
                AverageAmount = items.Any() ? items.Average(i => i.TotalPrice) : 0,
                MinAmount = items.Any() ? items.Min(i => i.TotalPrice) : 0,
                MaxAmount = items.Any() ? items.Max(i => i.TotalPrice) : 0,
                // PopularProductTypes removed - SaleItem entity doesn't have ProductType property
            };
        }
    }
} 
