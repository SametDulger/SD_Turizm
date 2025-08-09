using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SD_Turizm.Core.Entities;
using SD_Turizm.Core.Interfaces;
using SD_Turizm.Core.DTOs;

namespace SD_Turizm.Application.Services
{
    public class PackageService : IPackageService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PackageService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Package>> GetAllAsync()
        {
            return await _unitOfWork.Repository<Package>().GetAllAsync();
        }

        public async Task<Package?> GetByIdAsync(int id)
        {
            return await _unitOfWork.Repository<Package>().GetByIdAsync(id);
        }

        public async Task<Package> CreateAsync(Package package)
        {
            await _unitOfWork.Repository<Package>().AddAsync(package);
            await _unitOfWork.SaveChangesAsync();
            return package;
        }

        public async Task<Package> UpdateAsync(Package package)
        {
            await _unitOfWork.Repository<Package>().UpdateAsync(package);
            await _unitOfWork.SaveChangesAsync();
            return package;
        }

        public async Task DeleteAsync(int id)
        {
            await _unitOfWork.Repository<Package>().DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _unitOfWork.Repository<Package>().ExistsAsync(id);
        }

        // V2 Methods
        public async Task<PagedResult<Package>> GetPackagesWithPaginationAsync(PaginationDto pagination, string? searchTerm = null, string? destination = null, bool? isActive = null)
        {
            var packages = await _unitOfWork.Repository<Package>().GetAllAsync();
            
            // Apply filters
            if (!string.IsNullOrEmpty(searchTerm))
                packages = packages.Where(p => p.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase));
            
            // Destination filter removed - Package entity doesn't have Destination property
            
            if (isActive.HasValue)
                packages = packages.Where(p => p.IsActive == isActive.Value);

            var totalCount = packages.Count();
            var items = packages.Skip((pagination.Page - 1) * pagination.PageSize).Take(pagination.PageSize).ToList();

            return new PagedResult<Package>
            {
                Items = items,
                TotalCount = totalCount,
                Page = pagination.Page,
                PageSize = pagination.PageSize,
                TotalPages = (int)Math.Ceiling((double)totalCount / pagination.PageSize)
            };
        }

        public async Task<PagedResult<Package>> SearchPackagesAsync(PaginationDto pagination, string searchTerm, string? packageType = null)
        {
            var packages = await _unitOfWork.Repository<Package>().GetAllAsync();
            
            packages = packages.Where(p => p.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase));
            
            // PackageType filter removed - Package entity doesn't have PackageType property

            var totalCount = packages.Count();
            var items = packages.Skip((pagination.Page - 1) * pagination.PageSize).Take(pagination.PageSize).ToList();

            return new PagedResult<Package>
            {
                Items = items,
                TotalCount = totalCount,
                Page = pagination.Page,
                PageSize = pagination.PageSize,
                TotalPages = (int)Math.Ceiling((double)totalCount / pagination.PageSize)
            };
        }

        public async Task<object> GetPackageStatisticsAsync()
        {
            var packages = await _unitOfWork.Repository<Package>().GetAllAsync();
            
            return new
            {
                TotalCount = packages.Count(),
                ActiveCount = packages.Count(p => p.IsActive),
                InactiveCount = packages.Count(p => !p.IsActive),
                // Destinations and PackageTypes removed - Package entity doesn't have these properties
            };
        }
    }
} 
