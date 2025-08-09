using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SD_Turizm.Core.Entities;
using SD_Turizm.Core.Interfaces;
using SD_Turizm.Core.DTOs;

namespace SD_Turizm.Application.Services
{
    public class RentACarService : IRentACarService
    {
        private readonly IUnitOfWork _unitOfWork;

        public RentACarService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<RentACar>> GetAllAsync()
        {
            return await _unitOfWork.Repository<RentACar>().GetAllAsync();
        }

        public async Task<RentACar?> GetByIdAsync(int id)
        {
            return await _unitOfWork.Repository<RentACar>().GetByIdAsync(id);
        }

        public async Task<RentACar> CreateAsync(RentACar rentACar)
        {
            await _unitOfWork.Repository<RentACar>().AddAsync(rentACar);
            await _unitOfWork.SaveChangesAsync();
            return rentACar;
        }

        public async Task<RentACar> UpdateAsync(RentACar rentACar)
        {
            await _unitOfWork.Repository<RentACar>().UpdateAsync(rentACar);
            await _unitOfWork.SaveChangesAsync();
            return rentACar;
        }

        public async Task DeleteAsync(int id)
        {
            await _unitOfWork.Repository<RentACar>().DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _unitOfWork.Repository<RentACar>().ExistsAsync(id);
        }

        // V2 Methods
        public async Task<PagedResult<RentACar>> GetRentACarsWithPaginationAsync(PaginationDto pagination, string? searchTerm = null, string? location = null, bool? isActive = null)
        {
            var rentACars = await _unitOfWork.Repository<RentACar>().GetAllAsync();
            
            // Apply filters
            if (!string.IsNullOrEmpty(searchTerm))
                rentACars = rentACars.Where(r => r.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase));
            
            // Location filter removed - RentACar entity doesn't have Location property
            
            if (isActive.HasValue)
                rentACars = rentACars.Where(r => r.IsActive == isActive.Value);

            var totalCount = rentACars.Count();
            var items = rentACars.Skip((pagination.Page - 1) * pagination.PageSize).Take(pagination.PageSize).ToList();

            return new PagedResult<RentACar>
            {
                Items = items,
                TotalCount = totalCount,
                Page = pagination.Page,
                PageSize = pagination.PageSize,
                TotalPages = (int)Math.Ceiling((double)totalCount / pagination.PageSize)
            };
        }

        public async Task<PagedResult<RentACar>> SearchRentACarsAsync(PaginationDto pagination, string searchTerm, string? serviceType = null)
        {
            var rentACars = await _unitOfWork.Repository<RentACar>().GetAllAsync();
            
            rentACars = rentACars.Where(r => r.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase));
            
            // ServiceType filter removed - RentACar entity doesn't have ServiceType property

            var totalCount = rentACars.Count();
            var items = rentACars.Skip((pagination.Page - 1) * pagination.PageSize).Take(pagination.PageSize).ToList();

            return new PagedResult<RentACar>
            {
                Items = items,
                TotalCount = totalCount,
                Page = pagination.Page,
                PageSize = pagination.PageSize,
                TotalPages = (int)Math.Ceiling((double)totalCount / pagination.PageSize)
            };
        }

        public async Task<object> GetRentACarStatisticsAsync()
        {
            var rentACars = await _unitOfWork.Repository<RentACar>().GetAllAsync();
            
            return new
            {
                TotalCount = rentACars.Count(),
                ActiveCount = rentACars.Count(r => r.IsActive),
                InactiveCount = rentACars.Count(r => !r.IsActive),
                // Locations and ServiceTypes removed - RentACar entity doesn't have these properties
            };
        }
    }
} 
