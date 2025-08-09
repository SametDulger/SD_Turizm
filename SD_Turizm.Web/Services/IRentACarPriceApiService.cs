using SD_Turizm.Web.Models.DTOs;

namespace SD_Turizm.Web.Services
{
    public interface IRentACarPriceApiService
    {
        Task<List<RentACarPriceDto>?> GetAllRentACarPricesAsync();
        Task<RentACarPriceDto?> GetRentACarPriceByIdAsync(int id);
        Task<RentACarPriceDto?> CreateRentACarPriceAsync(RentACarPriceDto rentACarPrice);
        Task<RentACarPriceDto?> UpdateRentACarPriceAsync(int id, RentACarPriceDto rentACarPrice);
        Task<bool> DeleteRentACarPriceAsync(int id);
        Task<List<RentACarPriceDto>?> GetRentACarPricesByRentACarIdAsync(int rentACarId);
    }
}
