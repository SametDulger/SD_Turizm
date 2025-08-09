using System.Collections.Generic;
using System.Threading.Tasks;
using SD_Turizm.Core.Entities;
using SD_Turizm.Core.DTOs;

namespace SD_Turizm.Application.Services
{
    public interface ITourOperatorService
    {
        Task<IEnumerable<TourOperator>> GetAllTourOperatorsAsync();
        Task<TourOperator?> GetTourOperatorByIdAsync(int id);
        Task<TourOperator?> GetTourOperatorByCodeAsync(string code);
        Task<TourOperator> CreateTourOperatorAsync(TourOperator tourOperator);
        Task UpdateTourOperatorAsync(TourOperator tourOperator);
        Task DeleteTourOperatorAsync(int id);
        Task<bool> TourOperatorExistsAsync(int id);
        Task<bool> TourOperatorCodeExistsAsync(string code);
        
        // V2 Methods
        Task<PagedResult<TourOperator>> GetTourOperatorsWithPaginationAsync(PaginationDto pagination, string? searchTerm = null, string? region = null, bool? isActive = null);
        Task<PagedResult<TourOperator>> SearchTourOperatorsAsync(PaginationDto pagination, string region, string? serviceType = null);
        Task<object> GetTourOperatorStatisticsAsync();
        Task<int> BulkUpdateAsync(List<TourOperator> tourOperators);
    }
} 
