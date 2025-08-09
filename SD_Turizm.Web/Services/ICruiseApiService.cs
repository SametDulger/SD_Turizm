using SD_Turizm.Web.Models.DTOs;

namespace SD_Turizm.Web.Services
{
    public interface ICruiseApiService
    {
        Task<List<CruiseDto>> GetAllCruisesAsync();
        Task<CruiseDto?> GetCruiseByIdAsync(int id);
        Task<CruiseDto?> CreateCruiseAsync(CruiseDto cruise);
        Task<CruiseDto?> UpdateCruiseAsync(int id, CruiseDto cruise);
        Task<bool> DeleteCruiseAsync(int id);
    }
}
