using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SD_Turizm.Core.Entities;
using SD_Turizm.Core.Interfaces;
using SD_Turizm.Core.DTOs;

namespace SD_Turizm.Application.Services
{
    public class SalePersonService : ISalePersonService
    {
        private readonly IUnitOfWork _unitOfWork;

        public SalePersonService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<SalePerson>> GetAllAsync()
        {
            return await _unitOfWork.Repository<SalePerson>().GetAllAsync();
        }

        public async Task<SalePerson?> GetByIdAsync(int id)
        {
            return await _unitOfWork.Repository<SalePerson>().GetByIdAsync(id);
        }

        public async Task<SalePerson> CreateAsync(SalePerson salePerson)
        {
            await _unitOfWork.Repository<SalePerson>().AddAsync(salePerson);
            await _unitOfWork.SaveChangesAsync();
            return salePerson;
        }

        public async Task<SalePerson> UpdateAsync(SalePerson salePerson)
        {
            await _unitOfWork.Repository<SalePerson>().UpdateAsync(salePerson);
            await _unitOfWork.SaveChangesAsync();
            return salePerson;
        }

        public async Task DeleteAsync(int id)
        {
            await _unitOfWork.Repository<SalePerson>().DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _unitOfWork.Repository<SalePerson>().ExistsAsync(id);
        }

        // V2 Methods
        public async Task<PagedResult<SalePerson>> GetSalePersonsWithPaginationAsync(PaginationDto pagination, string? searchTerm = null, string? region = null, bool? isActive = null)
        {
            var salePersons = await _unitOfWork.Repository<SalePerson>().GetAllAsync();
            
            // Apply filters
            // Name and Region filters removed - SalePerson entity doesn't have these properties
            
            if (isActive.HasValue)
                salePersons = salePersons.Where(s => s.IsActive == isActive.Value);

            var totalCount = salePersons.Count();
            var items = salePersons.Skip((pagination.Page - 1) * pagination.PageSize).Take(pagination.PageSize).ToList();

            return new PagedResult<SalePerson>
            {
                Items = items,
                TotalCount = totalCount,
                Page = pagination.Page,
                PageSize = pagination.PageSize,
                TotalPages = (int)Math.Ceiling((double)totalCount / pagination.PageSize)
            };
        }

        public async Task<PagedResult<SalePerson>> SearchSalePersonsAsync(PaginationDto pagination, string searchTerm, string? serviceType = null)
        {
            var salePersons = await _unitOfWork.Repository<SalePerson>().GetAllAsync();
            
            // Region filter removed - SalePerson entity doesn't have Region property
            
            // ServiceType filter removed - SalePerson entity doesn't have ServiceType property

            var totalCount = salePersons.Count();
            var items = salePersons.Skip((pagination.Page - 1) * pagination.PageSize).Take(pagination.PageSize).ToList();

            return new PagedResult<SalePerson>
            {
                Items = items,
                TotalCount = totalCount,
                Page = pagination.Page,
                PageSize = pagination.PageSize,
                TotalPages = (int)Math.Ceiling((double)totalCount / pagination.PageSize)
            };
        }

        public async Task<object> GetSalePersonStatisticsAsync()
        {
            var salePersons = await _unitOfWork.Repository<SalePerson>().GetAllAsync();
            
            return new
            {
                TotalCount = salePersons.Count(),
                ActiveCount = salePersons.Count(s => s.IsActive),
                InactiveCount = salePersons.Count(s => !s.IsActive),
                // Regions and ServiceTypes removed - SalePerson entity doesn't have these properties
            };
        }
    }
} 
