using System.Collections.Generic;
using System.Threading.Tasks;
using SD_Turizm.Core.Entities;

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
    }
} 
