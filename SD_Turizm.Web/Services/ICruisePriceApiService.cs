using SD_Turizm.Web.Models.DTOs;

namespace SD_Turizm.Web.Services
{
    public interface ICruisePriceApiService
    {
        Task<List<CruisePriceDto>?> GetAllCruisePricesAsync();
        Task<CruisePriceDto?> GetCruisePriceByIdAsync(int id);
        Task<CruisePriceDto?> CreateCruisePriceAsync(CruisePriceDto cruisePrice);
        Task<CruisePriceDto?> UpdateCruisePriceAsync(int id, CruisePriceDto cruisePrice);
        Task<bool> DeleteCruisePriceAsync(int id);
        Task<List<CruisePriceDto>?> GetCruisePricesByCruiseIdAsync(int cruiseId);
    }
}
