using SD_Turizm.Web.Models.DTOs;

namespace SD_Turizm.Web.Services
{
    public interface IAirlineApiService
    {
        Task<List<AirlineDto>> GetAllAirlinesAsync();
        Task<AirlineDto?> GetAirlineByIdAsync(int id);
        Task<AirlineDto?> CreateAirlineAsync(AirlineDto airline);
        Task<AirlineDto?> UpdateAirlineAsync(int id, AirlineDto airline);
        Task<bool> DeleteAirlineAsync(int id);
    }
}
