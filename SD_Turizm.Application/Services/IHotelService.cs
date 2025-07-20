using System.Collections.Generic;
using System.Threading.Tasks;
using SD_Turizm.Core.Entities;

namespace SD_Turizm.Application.Services
{
    public interface IHotelService
    {
        Task<IEnumerable<Hotel>> GetAllHotelsAsync();
        Task<Hotel> GetHotelByIdAsync(int id);
        Task<Hotel> GetHotelByCodeAsync(string code);
        Task<Hotel> CreateHotelAsync(Hotel hotel);
        Task UpdateHotelAsync(Hotel hotel);
        Task DeleteHotelAsync(int id);
        Task<bool> HotelExistsAsync(int id);
        Task<bool> HotelCodeExistsAsync(string code);
    }
} 