using SD_Turizm.Web.Models.DTOs;

namespace SD_Turizm.Web.Services
{
    public interface IHotelPriceApiService
    {
        Task<List<HotelPriceDto>?> GetAllHotelPricesAsync();
        Task<HotelPriceDto?> GetHotelPriceByIdAsync(int id);
        Task<HotelPriceDto?> CreateHotelPriceAsync(HotelPriceDto hotelPrice);
        Task<HotelPriceDto?> UpdateHotelPriceAsync(int id, HotelPriceDto hotelPrice);
        Task<bool> DeleteHotelPriceAsync(int id);
        Task<List<HotelPriceDto>?> GetHotelPricesByHotelIdAsync(int hotelId);
    }
}
