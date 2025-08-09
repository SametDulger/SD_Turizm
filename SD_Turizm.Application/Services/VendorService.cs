using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SD_Turizm.Core.Entities;
using SD_Turizm.Core.Interfaces;
using SD_Turizm.Core.DTOs;
using Microsoft.EntityFrameworkCore;

namespace SD_Turizm.Application.Services
{
    public class VendorService : IVendorService
    {
        private readonly IUnitOfWork _unitOfWork;
        
        public VendorService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        
        public async Task<IEnumerable<VendorDto>> GetAllVendorsAsync()
        {
            var vendors = await _unitOfWork.Repository<Vendor>().GetAllAsync();
            return vendors.Select(MapToDto).ToList();
        }

        public async Task<VendorDto?> GetVendorByIdAsync(int id)
        {
            var vendor = await _unitOfWork.Repository<Vendor>().GetByIdAsync(id);
            return vendor != null ? MapToDto(vendor) : null;
        }

        public async Task<IEnumerable<VendorDto>> GetActiveVendorsAsync()
        {
            var vendors = await _unitOfWork.Repository<Vendor>().GetAllAsync();
            return vendors.Where(v => v.IsActive).Select(MapToDto).ToList();
        }

        public async Task<IEnumerable<VendorDto>> GetVendorsByTypeAsync(string vendorType)
        {
            var vendors = await _unitOfWork.Repository<Vendor>().GetAllAsync();
            return vendors.Where(v => v.GetType().Name.Equals(vendorType, StringComparison.OrdinalIgnoreCase))
                         .Select(MapToDto).ToList();
        }

        public async Task<PagedResult<VendorDto>> GetVendorsWithPaginationAsync(int page, int pageSize, string? searchTerm = null, string? vendorType = null, bool? isActive = null)
        {
            var query = _unitOfWork.Repository<Vendor>().GetQueryable();

            // Apply filters
            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(v => v.Name.Contains(searchTerm) || v.Code.Contains(searchTerm));
            }

            if (!string.IsNullOrEmpty(vendorType))
            {
                query = query.Where(v => v.GetType().Name.Equals(vendorType, StringComparison.OrdinalIgnoreCase));
            }

            if (isActive.HasValue)
            {
                query = query.Where(v => v.IsActive == isActive.Value);
            }

            var totalItems = await query.CountAsync();
            var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            var vendors = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<VendorDto>
            {
                Items = vendors.Select(MapToDto).ToList(),
                TotalCount = totalItems,
                Page = page,
                PageSize = pageSize,
                TotalPages = totalPages
            };
        }

        public async Task<object> GetVendorStatisticsAsync()
        {
            var vendors = await _unitOfWork.Repository<Vendor>().GetAllAsync();
            
            var stats = vendors.GroupBy(v => v.GetType().Name)
                              .Select(g => new { Type = g.Key, Count = g.Count() })
                              .ToList();

            var totalVendorsCount = vendors.Count();
            var activeVendorsCount = vendors.Where(v => v.IsActive).Count();
            return new
            {
                TotalVendors = totalVendorsCount,
                ActiveVendors = activeVendorsCount,
                VendorTypes = stats
            };
        }

        public async Task<VendorDto> CreateVendorAsync(VendorDto vendorDto)
        {
            var vendor = MapToEntity(vendorDto);
            await _unitOfWork.Repository<Vendor>().AddAsync(vendor);
            await _unitOfWork.SaveChangesAsync();
            return MapToDto(vendor);
        }

        public async Task<VendorDto?> UpdateVendorAsync(VendorDto vendorDto)
        {
            var existingVendor = await _unitOfWork.Repository<Vendor>().GetByIdAsync(vendorDto.Id);
            if (existingVendor == null)
                return null;

            UpdateVendorFromDto(existingVendor, vendorDto);
            await _unitOfWork.Repository<Vendor>().UpdateAsync(existingVendor);
            await _unitOfWork.SaveChangesAsync();
            return MapToDto(existingVendor);
        }

        public async Task<bool> DeleteVendorAsync(int id)
        {
            await _unitOfWork.Repository<Vendor>().DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<VendorDto?> ToggleVendorStatusAsync(int id)
        {
            var vendor = await _unitOfWork.Repository<Vendor>().GetByIdAsync(id);
            if (vendor == null)
                return null;

            vendor.IsActive = !vendor.IsActive;
            await _unitOfWork.Repository<Vendor>().UpdateAsync(vendor);
            await _unitOfWork.SaveChangesAsync();
            return MapToDto(vendor);
        }

        private VendorDto MapToDto(Vendor vendor)
        {
            return new VendorDto
            {
                Id = vendor.Id,
                Code = vendor.Code,
                Name = vendor.Name,
                Phone = vendor.Phone,
                Email = vendor.Email,
                Address = vendor.Address,
                Country = vendor.Country,
                Description = vendor.Description,
                IsActive = vendor.IsActive,
                CreatedDate = vendor.CreatedDate,
                VendorType = vendor.GetType().Name
            };
        }

        private Vendor MapToEntity(VendorDto dto)
        {
            // Bu metod Vendor'ın abstract olması nedeniyle tam implementasyon gerektirir
            // Şimdilik basit bir mapping
            return new Hotel // Örnek olarak Hotel kullanıyoruz
            {
                Id = dto.Id,
                Code = dto.Code,
                Name = dto.Name,
                Phone = dto.Phone,
                Email = dto.Email,
                Address = dto.Address,
                Country = dto.Country,
                Description = dto.Description,
                IsActive = dto.IsActive,
                CreatedDate = dto.CreatedDate
            };
        }

        private void UpdateVendorFromDto(Vendor vendor, VendorDto dto)
        {
            vendor.Code = dto.Code;
            vendor.Name = dto.Name;
            vendor.Phone = dto.Phone;
            vendor.Email = dto.Email;
            vendor.Address = dto.Address;
            vendor.Country = dto.Country;
            vendor.Description = dto.Description;
            vendor.IsActive = dto.IsActive;
        }
    }
}
