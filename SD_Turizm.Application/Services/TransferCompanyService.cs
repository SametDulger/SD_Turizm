using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SD_Turizm.Core.Entities;
using SD_Turizm.Core.Interfaces;
using SD_Turizm.Core.DTOs;

namespace SD_Turizm.Application.Services
{
    public class TransferCompanyService : ITransferCompanyService
    {
        private readonly IUnitOfWork _unitOfWork;
        public TransferCompanyService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IEnumerable<TransferCompany>> GetAllAsync()
        {
            return await _unitOfWork.Repository<TransferCompany>().GetAllAsync();
        }
        public async Task<TransferCompany?> GetByIdAsync(int id)
        {
            return await _unitOfWork.Repository<TransferCompany>().GetByIdAsync(id);
        }
        public async Task<TransferCompany> CreateAsync(TransferCompany entity)
        {
            await _unitOfWork.Repository<TransferCompany>().AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            return entity;
        }
        public async Task UpdateAsync(TransferCompany entity)
        {
            await _unitOfWork.Repository<TransferCompany>().UpdateAsync(entity);
            await _unitOfWork.SaveChangesAsync();
        }
        public async Task DeleteAsync(int id)
        {
            await _unitOfWork.Repository<TransferCompany>().DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
        }
        public async Task<bool> ExistsAsync(int id)
        {
            return await _unitOfWork.Repository<TransferCompany>().ExistsAsync(id);
        }

        // V2 Methods
        public async Task<PagedResult<TransferCompany>> GetTransferCompaniesWithPaginationAsync(PaginationDto pagination, string? searchTerm = null, string? region = null, bool? isActive = null)
        {
            var companies = await _unitOfWork.Repository<TransferCompany>().GetAllAsync();
            
            // Apply filters
            if (!string.IsNullOrEmpty(searchTerm))
                companies = companies.Where(c => c.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase));
            
            if (!string.IsNullOrEmpty(region))
                companies = companies.Where(c => c.Region.Contains(region, StringComparison.OrdinalIgnoreCase));
            
            if (isActive.HasValue)
                companies = companies.Where(c => c.IsActive == isActive.Value);

            var totalCount = companies.Count();
            var items = companies.Skip((pagination.Page - 1) * pagination.PageSize).Take(pagination.PageSize).ToList();

            return new PagedResult<TransferCompany>
            {
                Items = items,
                TotalCount = totalCount,
                Page = pagination.Page,
                PageSize = pagination.PageSize,
                TotalPages = (int)Math.Ceiling((double)totalCount / pagination.PageSize)
            };
        }

        public async Task<PagedResult<TransferCompany>> SearchTransferCompaniesAsync(PaginationDto pagination, string region, string? serviceType = null)
        {
            var companies = await _unitOfWork.Repository<TransferCompany>().GetAllAsync();
            
            companies = companies.Where(c => c.Region.Contains(region, StringComparison.OrdinalIgnoreCase));
            
            // ServiceType filter removed - TransferCompany entity doesn't have ServiceType property

            var totalCount = companies.Count();
            var items = companies.Skip((pagination.Page - 1) * pagination.PageSize).Take(pagination.PageSize).ToList();

            return new PagedResult<TransferCompany>
            {
                Items = items,
                TotalCount = totalCount,
                Page = pagination.Page,
                PageSize = pagination.PageSize,
                TotalPages = (int)Math.Ceiling((double)totalCount / pagination.PageSize)
            };
        }

        public async Task<object> GetTransferCompanyStatisticsAsync()
        {
            var companies = await _unitOfWork.Repository<TransferCompany>().GetAllAsync();
            
            return new
            {
                TotalTransferCompanies = companies.Count(),
                ActiveTransferCompanies = companies.Count(c => c.IsActive),
                PopularRegions = companies.GroupBy(c => c.Region)
                    .OrderByDescending(g => g.Count())
                    .Take(5)
                    .Select(g => new { Region = g.Key, Count = g.Count() }),
                // PopularServiceTypes removed - TransferCompany entity doesn't have ServiceType property
            };
        }
    }
} 
