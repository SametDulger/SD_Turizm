using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SD_Turizm.Core.Entities;
using SD_Turizm.Core.Interfaces;
using SD_Turizm.Core.DTOs;

namespace SD_Turizm.Application.Services
{
    public class HotelService : IHotelService
    {
        private readonly IUnitOfWork _unitOfWork;

        public HotelService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Hotel>> GetAllHotelsAsync()
        {
            return await _unitOfWork.Repository<Hotel>().GetAllAsync();
        }

        public async Task<Hotel?> GetHotelByIdAsync(int id)
        {
            return await _unitOfWork.Repository<Hotel>().GetByIdAsync(id);
        }

        public async Task<Hotel?> GetHotelByCodeAsync(string code)
        {
            var hotels = await _unitOfWork.Repository<Hotel>().FindAsync(h => h.Code == code);
            return hotels.FirstOrDefault();
        }

        public async Task<Hotel> CreateHotelAsync(Hotel hotel)
        {
            await _unitOfWork.Repository<Hotel>().AddAsync(hotel);
            await _unitOfWork.SaveChangesAsync();
            return hotel;
        }

        public async Task UpdateHotelAsync(Hotel hotel)
        {
            await _unitOfWork.Repository<Hotel>().UpdateAsync(hotel);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteHotelAsync(int id)
        {
            await _unitOfWork.Repository<Hotel>().DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<bool> HotelExistsAsync(int id)
        {
            return await _unitOfWork.Repository<Hotel>().ExistsAsync(id);
        }

        public async Task<bool> HotelCodeExistsAsync(string code)
        {
            var hotels = await _unitOfWork.Repository<Hotel>().FindAsync(h => h.Code == code);
            return hotels.Any();
        }

        // V2 Methods
        public async Task<PagedResult<Hotel>> GetHotelsWithPaginationAsync(PaginationDto pagination, string? searchTerm = null, string? location = null, int? stars = null, string? amenities = null)
        {
            var hotels = await GetAllHotelsAsync();
            var hotelsList = hotels.ToList();

            // Apply filters
            if (!string.IsNullOrEmpty(searchTerm))
                hotelsList = hotelsList.Where(h => h.Name?.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) == true || h.Code?.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) == true).ToList();
            if (!string.IsNullOrEmpty(location))
                hotelsList = hotelsList.Where(h => h.Location?.Contains(location, StringComparison.OrdinalIgnoreCase) == true).ToList();
            if (stars.HasValue)
                hotelsList = hotelsList.Where(h => h.Stars == stars.Value).ToList();
            if (!string.IsNullOrEmpty(amenities))
                hotelsList = hotelsList.Where(h => h.Amenities?.Contains(amenities, StringComparison.OrdinalIgnoreCase) == true).ToList();

            var totalCount = hotelsList.Count;
            var items = hotelsList.Skip((pagination.Page - 1) * pagination.PageSize).Take(pagination.PageSize).ToList();

            return new PagedResult<Hotel>
            {
                Items = items,
                TotalCount = totalCount,
                Page = pagination.Page,
                PageSize = pagination.PageSize,
                TotalPages = (int)Math.Ceiling((double)totalCount / pagination.PageSize)
            };
        }

        public async Task<PagedResult<Hotel>> SearchHotelsAsync(PaginationDto pagination, string? location = null, int? stars = null, string? amenities = null)
        {
            return await GetHotelsWithPaginationAsync(pagination, null, location, stars, amenities);
        }

        public async Task<object> CheckAvailabilityAsync(int hotelId, DateTime startDate, DateTime endDate)
        {
            // Mock implementation
            return await Task.FromResult(new
            {
                HotelId = hotelId,
                StartDate = startDate,
                EndDate = endDate,
                Available = true,
                AvailableRooms = 15,
                PricePerNight = 250.0m
            });
        }

        public async Task<object> GetHotelStatisticsAsync()
        {
            var hotels = await GetAllHotelsAsync();
            var hotelsList = hotels.ToList();

            return new
            {
                TotalHotels = hotelsList.Count,
                AverageStars = hotelsList.Any() ? hotelsList.Average(h => h.Stars) : 0,
                HotelsByLocation = hotelsList.GroupBy(h => h.Location).Select(g => new { Location = g.Key, Count = g.Count() }).ToList(),
                TopRatedHotels = hotelsList.OrderByDescending(h => h.Stars).Take(5).ToList()
            };
        }

        public async Task<int> BulkUpdateAsync(List<Hotel> hotels)
        {
            foreach (var hotel in hotels)
            {
                await UpdateHotelAsync(hotel);
            }
            return hotels.Count;
        }

        public async Task<Hotel> CreateAsync(Hotel hotel)
        {
            return await CreateHotelAsync(hotel);
        }

        public async Task UpdateAsync(Hotel hotel)
        {
            await UpdateHotelAsync(hotel);
        }

        public async Task DeleteAsync(int id)
        {
            await DeleteHotelAsync(id);
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await HotelExistsAsync(id);
        }

        public async Task<Hotel?> GetByIdAsync(int id)
        {
            return await GetHotelByIdAsync(id);
        }

        public async Task<Hotel?> GetByCodeAsync(string code)
        {
            return await GetHotelByCodeAsync(code);
        }
    }
} 
