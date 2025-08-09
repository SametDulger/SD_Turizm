using SD_Turizm.Web.Models.DTOs;

namespace SD_Turizm.Web.Services
{
    public interface ITourApiService
    {
        Task<List<TourDto>> GetAllToursAsync();
        Task<TourDto?> GetTourByIdAsync(int id);
        Task<TourDto?> CreateTourAsync(TourDto tour);
        Task<TourDto?> UpdateTourAsync(int id, TourDto tour);
        Task<bool> DeleteTourAsync(int id);
    }
}
