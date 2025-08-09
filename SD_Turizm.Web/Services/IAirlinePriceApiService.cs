using SD_Turizm.Web.Models.DTOs;

namespace SD_Turizm.Web.Services
{
    public interface IAirlinePriceApiService
    {
        Task<List<AirlinePriceDto>?> GetAllAirlinePricesAsync();
        Task<AirlinePriceDto?> GetAirlinePriceByIdAsync(int id);
        Task<AirlinePriceDto?> CreateAirlinePriceAsync(AirlinePriceDto airlinePrice);
        Task<AirlinePriceDto?> UpdateAirlinePriceAsync(int id, AirlinePriceDto airlinePrice);
        Task<bool> DeleteAirlinePriceAsync(int id);
        Task<List<AirlinePriceDto>?> GetAirlinePricesByAirlineIdAsync(int airlineId);
    }
}
