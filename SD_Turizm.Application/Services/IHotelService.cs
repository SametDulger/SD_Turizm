using System.Collections.Generic;
using System.Threading.Tasks;
using SD_Turizm.Core.Entities;
using SD_Turizm.Core.DTOs;

namespace SD_Turizm.Application.Services
{
    public interface IHotelService
    {
        Task<IEnumerable<Hotel>> GetAllHotelsAsync();
        Task<Hotel?> GetHotelByIdAsync(int id);
        Task<Hotel?> GetHotelByCodeAsync(string code);
        Task<Hotel> CreateHotelAsync(Hotel hotel);
        Task UpdateHotelAsync(Hotel hotel);
        Task DeleteHotelAsync(int id);
        Task<bool> HotelExistsAsync(int id);
        Task<bool> HotelCodeExistsAsync(string code);
        
        // V2 Methods
        Task<PagedResult<Hotel>> GetHotelsWithPaginationAsync(PaginationDto pagination, string? searchTerm = null, string? location = null, int? stars = null, string? amenities = null);
        Task<PagedResult<Hotel>> SearchHotelsAsync(PaginationDto pagination, string? location = null, int? stars = null, string? amenities = null);
        Task<object> CheckAvailabilityAsync(int hotelId, DateTime startDate, DateTime endDate);
        Task<object> GetHotelStatisticsAsync();
        Task<int> BulkUpdateAsync(List<Hotel> hotels);
        Task<Hotel> CreateAsync(Hotel hotel);
        Task UpdateAsync(Hotel hotel);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<Hotel?> GetByIdAsync(int id);
        Task<Hotel?> GetByCodeAsync(string code);
    }
} 
