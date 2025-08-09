using System.Collections.Generic;
using System.Threading.Tasks;
using SD_Turizm.Core.Entities;
using SD_Turizm.Core.DTOs;

namespace SD_Turizm.Application.Services
{
    public interface IRentACarService
    {
        Task<IEnumerable<RentACar>> GetAllAsync();
        Task<RentACar?> GetByIdAsync(int id);
        Task<RentACar> CreateAsync(RentACar rentACar);
        Task<RentACar> UpdateAsync(RentACar rentACar);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        
        // V2 Methods
        Task<PagedResult<RentACar>> GetRentACarsWithPaginationAsync(PaginationDto pagination, string? searchTerm = null, string? location = null, bool? isActive = null);
        Task<PagedResult<RentACar>> SearchRentACarsAsync(PaginationDto pagination, string location, string? serviceType = null);
        Task<object> GetRentACarStatisticsAsync();
    }
} 
