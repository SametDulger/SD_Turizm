using SD_Turizm.Web.Models.DTOs;

namespace SD_Turizm.Web.Services
{
    public interface IRentACarApiService
    {
        Task<List<RentACarDto>> GetAllRentACarsAsync();
        Task<RentACarDto?> GetRentACarByIdAsync(int id);
        Task<RentACarDto?> CreateRentACarAsync(RentACarDto rentACar);
        Task<RentACarDto?> UpdateRentACarAsync(int id, RentACarDto rentACar);
        Task<bool> DeleteRentACarAsync(int id);
    }
}
