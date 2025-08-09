using SD_Turizm.Web.Models.DTOs;

namespace SD_Turizm.Web.Services
{
    public interface IHotelApiService
    {
        Task<List<HotelDto>> GetAllHotelsAsync();
        Task<HotelDto?> GetHotelByIdAsync(int id);
        Task<HotelDto?> CreateHotelAsync(HotelDto hotel);
        Task<HotelDto?> UpdateHotelAsync(int id, HotelDto hotel);
        Task<bool> DeleteHotelAsync(int id);
    }
}
