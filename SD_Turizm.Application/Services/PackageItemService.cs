using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SD_Turizm.Core.Entities;
using SD_Turizm.Core.Interfaces;
using SD_Turizm.Core.DTOs;

namespace SD_Turizm.Application.Services
{
    public class PackageItemService : IPackageItemService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PackageItemService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<PackageItem>> GetAllAsync()
        {
            return await _unitOfWork.Repository<PackageItem>().GetAllAsync();
        }

        public async Task<PackageItem?> GetByIdAsync(int id)
        {
            return await _unitOfWork.Repository<PackageItem>().GetByIdAsync(id);
        }

        public async Task<PackageItem> CreateAsync(PackageItem packageItem)
        {
            await _unitOfWork.Repository<PackageItem>().AddAsync(packageItem);
            await _unitOfWork.SaveChangesAsync();
            return packageItem;
        }

        public async Task<PackageItem> UpdateAsync(PackageItem packageItem)
        {
            await _unitOfWork.Repository<PackageItem>().UpdateAsync(packageItem);
            await _unitOfWork.SaveChangesAsync();
            return packageItem;
        }

        public async Task DeleteAsync(int id)
        {
            await _unitOfWork.Repository<PackageItem>().DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _unitOfWork.Repository<PackageItem>().ExistsAsync(id);
        }

        // V2 Methods
        public async Task<PagedResult<PackageItem>> GetPackageItemsWithPaginationAsync(PaginationDto pagination, int? packageId = null, string? itemType = null, decimal? minPrice = null, decimal? maxPrice = null)
        {
            var items = await _unitOfWork.Repository<PackageItem>().GetAllAsync();
            
            // Apply filters
            if (packageId.HasValue)
                items = items.Where(i => i.PackageId == packageId.Value);
            
            if (!string.IsNullOrEmpty(itemType))
                items = items.Where(i => i.ItemType.Contains(itemType, StringComparison.OrdinalIgnoreCase));
            
            if (minPrice.HasValue)
                items = items.Where(i => i.UnitPrice >= minPrice.Value);
            
            if (maxPrice.HasValue)
                items = items.Where(i => i.UnitPrice <= maxPrice.Value);

            var totalCount = items.Count();
            var result = items.Skip((pagination.Page - 1) * pagination.PageSize).Take(pagination.PageSize).ToList();

            return new PagedResult<PackageItem>
            {
                Items = result,
                TotalCount = totalCount,
                Page = pagination.Page,
                PageSize = pagination.PageSize,
                TotalPages = (int)Math.Ceiling((double)totalCount / pagination.PageSize)
            };
        }

        public async Task<IEnumerable<PackageItem>> GetItemsByPackageIdAsync(int packageId)
        {
            var items = await _unitOfWork.Repository<PackageItem>().GetAllAsync();
            return items.Where(i => i.PackageId == packageId).ToList();
        }

        public async Task<object> GetPackageItemStatisticsAsync()
        {
            var items = await _unitOfWork.Repository<PackageItem>().GetAllAsync();
            
            return new
            {
                TotalCount = items.Count(),
                AveragePrice = items.Any() ? items.Average(i => i.UnitPrice) : 0,
                TotalValue = items.Sum(i => i.UnitPrice),
                ItemTypes = items.GroupBy(i => i.ItemType).Select(g => new { ItemType = g.Key, Count = g.Count(), TotalPrice = g.Sum(i => i.UnitPrice) }),
                PackageDistribution = items.GroupBy(i => i.PackageId).Select(g => new { PackageId = g.Key, ItemCount = g.Count(), TotalPrice = g.Sum(i => i.UnitPrice) })
            };
        }
    }
} 
