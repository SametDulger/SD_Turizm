using SD_Turizm.Core.Entities;
using SD_Turizm.Core.Interfaces;
using SD_Turizm.Core.DTOs;

namespace SD_Turizm.Application.Services
{
    public class TourService : ITourService
    {
        private readonly IUnitOfWork _unitOfWork;

        public TourService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Tour>> GetAllAsync()
        {
            return await _unitOfWork.Repository<Tour>().GetAllAsync();
        }

        public async Task<Tour?> GetByIdAsync(int id)
        {
            return await _unitOfWork.Repository<Tour>().GetByIdAsync(id);
        }

        public async Task<Tour> CreateAsync(Tour entity)
        {
            await _unitOfWork.Repository<Tour>().AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            return entity;
        }

        public async Task<Tour> UpdateAsync(Tour entity)
        {
            await _unitOfWork.Repository<Tour>().UpdateAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            return entity;
        }

        public async Task DeleteAsync(int id)
        {
            await _unitOfWork.Repository<Tour>().DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _unitOfWork.Repository<Tour>().ExistsAsync(id);
        }

        // V2 Methods
        public async Task<PagedResult<Tour>> GetToursWithPaginationAsync(PaginationDto pagination, string? searchTerm = null, string? destination = null, int? duration = null, bool? isActive = null)
        {
            var tours = await _unitOfWork.Repository<Tour>().GetAllAsync();
            
            // Apply filters
            if (!string.IsNullOrEmpty(searchTerm))
                tours = tours.Where(t => t.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase));
            
            if (!string.IsNullOrEmpty(destination))
                tours = tours.Where(t => t.Region.Contains(destination, StringComparison.OrdinalIgnoreCase));
            
            if (duration.HasValue)
                tours = tours.Where(t => t.Duration.Contains(duration.Value.ToString(), StringComparison.OrdinalIgnoreCase));
            
            if (isActive.HasValue)
                tours = tours.Where(t => t.IsActive == isActive.Value);

            var totalCount = tours.Count();
            var items = tours.Skip((pagination.Page - 1) * pagination.PageSize).Take(pagination.PageSize).ToList();

            return new PagedResult<Tour>
            {
                Items = items,
                TotalCount = totalCount,
                Page = pagination.Page,
                PageSize = pagination.PageSize,
                TotalPages = (int)Math.Ceiling((double)totalCount / pagination.PageSize)
            };
        }

        public async Task<Tour?> GetByCodeAsync(string code)
        {
            var tours = await _unitOfWork.Repository<Tour>().GetAllAsync();
            return tours.FirstOrDefault(t => t.Code == code);
        }

        public async Task<PagedResult<Tour>> SearchToursAsync(PaginationDto pagination, string destination, int? duration = null, decimal? minPrice = null, decimal? maxPrice = null)
        {
            var tours = await _unitOfWork.Repository<Tour>().GetAllAsync();
            
            tours = tours.Where(t => t.Region.Contains(destination, StringComparison.OrdinalIgnoreCase));
            
            if (duration.HasValue)
                tours = tours.Where(t => t.Duration.Contains(duration.Value.ToString(), StringComparison.OrdinalIgnoreCase));
            
            // Note: Price filtering would need to be implemented through TourPrices collection
            // For now, we'll skip price filtering since it's not directly available on Tour entity

            var totalCount = tours.Count();
            var items = tours.Skip((pagination.Page - 1) * pagination.PageSize).Take(pagination.PageSize).ToList();

            return new PagedResult<Tour>
            {
                Items = items,
                TotalCount = totalCount,
                Page = pagination.Page,
                PageSize = pagination.PageSize,
                TotalPages = (int)Math.Ceiling((double)totalCount / pagination.PageSize)
            };
        }

        public async Task<object> GetTourStatisticsAsync()
        {
            var tours = await _unitOfWork.Repository<Tour>().GetAllAsync();
            
            return new
            {
                TotalTours = tours.Count(),
                ActiveTours = tours.Count(t => t.IsActive),
                PopularRegions = tours.GroupBy(t => t.Region)
                    .OrderByDescending(g => g.Count())
                    .Take(5)
                    .Select(g => new { Region = g.Key, Count = g.Count() }),
                PopularVehicleTypes = tours.GroupBy(t => t.VehicleType)
                    .OrderByDescending(g => g.Count())
                    .Take(5)
                    .Select(g => new { VehicleType = g.Key, Count = g.Count() })
            };
        }

        public async Task<int> BulkUpdateAsync(List<Tour> tours)
        {
            foreach (var tour in tours)
            {
                await _unitOfWork.Repository<Tour>().UpdateAsync(tour);
            }
            await _unitOfWork.SaveChangesAsync();
            return tours.Count;
        }
    }
} 
