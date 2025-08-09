using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SD_Turizm.Core.Entities;
using SD_Turizm.Core.Interfaces;
using SD_Turizm.Core.DTOs;

namespace SD_Turizm.Application.Services
{
    public class AddressService : IAddressService
    {
        private readonly IUnitOfWork _unitOfWork;
        public AddressService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IEnumerable<Address>> GetAllAsync()
        {
            return await _unitOfWork.Repository<Address>().GetAllAsync();
        }
        public async Task<Address?> GetByIdAsync(int id)
        {
            return await _unitOfWork.Repository<Address>().GetByIdAsync(id);
        }
        public async Task<Address> CreateAsync(Address address)
        {
            await _unitOfWork.Repository<Address>().AddAsync(address);
            await _unitOfWork.SaveChangesAsync();
            return address;
        }
        public async Task<Address> UpdateAsync(Address address)
        {
            await _unitOfWork.Repository<Address>().UpdateAsync(address);
            await _unitOfWork.SaveChangesAsync();
            return address;
        }
        public async Task DeleteAsync(int id)
        {
            await _unitOfWork.Repository<Address>().DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
        }
        public async Task<bool> ExistsAsync(int id)
        {
            return await _unitOfWork.Repository<Address>().ExistsAsync(id);
        }

        // V2 Methods
        public async Task<PagedResult<Address>> GetAddressesWithPaginationAsync(PaginationDto pagination, string? searchTerm = null, string? city = null, string? country = null)
        {
            var addresses = await _unitOfWork.Repository<Address>().GetAllAsync();

            // Apply filters
            if (!string.IsNullOrEmpty(searchTerm))
                addresses = addresses.Where(a => a.Street.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) || 
                                               a.City.Contains(searchTerm, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrEmpty(city))
                addresses = addresses.Where(a => a.City.Contains(city, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrEmpty(country))
                addresses = addresses.Where(a => a.Country.Contains(country, StringComparison.OrdinalIgnoreCase));

            var totalCount = addresses.Count();
            var items = addresses.Skip((pagination.Page - 1) * pagination.PageSize).Take(pagination.PageSize).ToList();

            return new PagedResult<Address>
            {
                Items = items,
                TotalCount = totalCount,
                Page = pagination.Page,
                PageSize = pagination.PageSize,
                TotalPages = (int)Math.Ceiling((double)totalCount / pagination.PageSize)
            };
        }

        public async Task<PagedResult<Address>> SearchAddressesAsync(PaginationDto pagination, string city, string? country = null)
        {
            var addresses = await _unitOfWork.Repository<Address>().GetAllAsync();

            addresses = addresses.Where(a => a.City.Contains(city, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrEmpty(country))
                addresses = addresses.Where(a => a.Country.Contains(country, StringComparison.OrdinalIgnoreCase));

            var totalCount = addresses.Count();
            var items = addresses.Skip((pagination.Page - 1) * pagination.PageSize).Take(pagination.PageSize).ToList();

            return new PagedResult<Address>
            {
                Items = items,
                TotalCount = totalCount,
                Page = pagination.Page,
                PageSize = pagination.PageSize,
                TotalPages = (int)Math.Ceiling((double)totalCount / pagination.PageSize)
            };
        }

        public async Task<object> GetAddressStatisticsAsync()
        {
            var addresses = await _unitOfWork.Repository<Address>().GetAllAsync();

            return new
            {
                TotalAddresses = addresses.Count(),
                Countries = addresses.GroupBy(a => a.Country).Count(),
                Cities = addresses.GroupBy(a => a.City).Count(),
                PopularCountries = addresses.GroupBy(a => a.Country)
                    .OrderByDescending(g => g.Count())
                    .Take(5)
                    .Select(g => new { Country = g.Key, Count = g.Count() }),
                PopularCities = addresses.GroupBy(a => a.City)
                    .OrderByDescending(g => g.Count())
                    .Take(5)
                    .Select(g => new { City = g.Key, Count = g.Count() })
            };
        }

        public async Task<List<Address>> BulkUpdateAsync(List<Address> addresses)
        {
            var updatedAddresses = new List<Address>();

            foreach (var address in addresses)
            {
                await _unitOfWork.Repository<Address>().UpdateAsync(address);
                updatedAddresses.Add(address);
            }

            await _unitOfWork.SaveChangesAsync();
            return updatedAddresses;
        }
    }
} 
