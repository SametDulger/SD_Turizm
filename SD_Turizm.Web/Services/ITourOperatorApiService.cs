using SD_Turizm.Web.Models.DTOs;

namespace SD_Turizm.Web.Services
{
    public interface ITourOperatorApiService
    {
        Task<List<TourOperatorDto>> GetAllTourOperatorsAsync();
        Task<TourOperatorDto?> GetTourOperatorByIdAsync(int id);
        Task<TourOperatorDto?> CreateTourOperatorAsync(TourOperatorDto tourOperator);
        Task<TourOperatorDto?> UpdateTourOperatorAsync(int id, TourOperatorDto tourOperator);
        Task<bool> DeleteTourOperatorAsync(int id);
    }
}
