using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SD_Turizm.Core.Entities;
using SD_Turizm.Core.Interfaces;
using SD_Turizm.Core.DTOs;

namespace SD_Turizm.Application.Services
{
    public class GuideService : IGuideService
    {
        private readonly IUnitOfWork _unitOfWork;

        public GuideService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Guide>> GetAllAsync()
        {
            return await _unitOfWork.Repository<Guide>().GetAllAsync();
        }

        public async Task<Guide?> GetByIdAsync(int id)
        {
            return await _unitOfWork.Repository<Guide>().GetByIdAsync(id);
        }

        public async Task<Guide> CreateAsync(Guide guide)
        {
            await _unitOfWork.Repository<Guide>().AddAsync(guide);
            await _unitOfWork.SaveChangesAsync();
            return guide;
        }

        public async Task<Guide> UpdateAsync(Guide guide)
        {
            await _unitOfWork.Repository<Guide>().UpdateAsync(guide);
            await _unitOfWork.SaveChangesAsync();
            return guide;
        }

        public async Task DeleteAsync(int id)
        {
            await _unitOfWork.Repository<Guide>().DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _unitOfWork.Repository<Guide>().ExistsAsync(id);
        }

        // V2 Methods
        public async Task<PagedResult<Guide>> GetGuidesWithPaginationAsync(PaginationDto pagination, string? searchTerm = null, string? region = null, bool? isActive = null)
        {
            var guides = await _unitOfWork.Repository<Guide>().GetAllAsync();
            
            // Apply filters
            if (!string.IsNullOrEmpty(searchTerm))
                guides = guides.Where(g => g.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase));
            
            // Region filter removed - Guide entity doesn't have Region property
            
            if (isActive.HasValue)
                guides = guides.Where(g => g.IsActive == isActive.Value);

            var totalCount = guides.Count();
            var items = guides.Skip((pagination.Page - 1) * pagination.PageSize).Take(pagination.PageSize).ToList();

            return new PagedResult<Guide>
            {
                Items = items,
                TotalCount = totalCount,
                Page = pagination.Page,
                PageSize = pagination.PageSize,
                TotalPages = (int)Math.Ceiling((double)totalCount / pagination.PageSize)
            };
        }

        public async Task<PagedResult<Guide>> SearchGuidesAsync(PaginationDto pagination, string searchTerm, string? language = null)
        {
            var guides = await _unitOfWork.Repository<Guide>().GetAllAsync();
            
            guides = guides.Where(g => g.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase));
            
            if (!string.IsNullOrEmpty(language))
                guides = guides.Where(g => g.Languages.Contains(language, StringComparison.OrdinalIgnoreCase));

            var totalCount = guides.Count();
            var items = guides.Skip((pagination.Page - 1) * pagination.PageSize).Take(pagination.PageSize).ToList();

            return new PagedResult<Guide>
            {
                Items = items,
                TotalCount = totalCount,
                Page = pagination.Page,
                PageSize = pagination.PageSize,
                TotalPages = (int)Math.Ceiling((double)totalCount / pagination.PageSize)
            };
        }

        public async Task<object> GetGuideStatisticsAsync()
        {
            var guides = await _unitOfWork.Repository<Guide>().GetAllAsync();
            
            return new
            {
                TotalCount = guides.Count(),
                ActiveCount = guides.Count(g => g.IsActive),
                InactiveCount = guides.Count(g => !g.IsActive),
                // Regions removed - Guide entity doesn't have Region property
                Languages = guides.GroupBy(g => g.Languages).Select(g => new { Language = g.Key, Count = g.Count() })
            };
        }
    }
} 
