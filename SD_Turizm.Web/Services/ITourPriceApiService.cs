using SD_Turizm.Web.Models.DTOs;

namespace SD_Turizm.Web.Services
{
    public interface ITourPriceApiService
    {
        Task<List<TourPriceDto>?> GetAllTourPricesAsync();
        Task<TourPriceDto?> GetTourPriceByIdAsync(int id);
        Task<TourPriceDto?> CreateTourPriceAsync(TourPriceDto tourPrice);
        Task<TourPriceDto?> UpdateTourPriceAsync(int id, TourPriceDto tourPrice);
        Task<bool> DeleteTourPriceAsync(int id);
        Task<List<TourPriceDto>?> GetTourPricesByTourIdAsync(int tourId);
    }
}
