using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SD_Turizm.Core.Entities;
using SD_Turizm.Core.Interfaces;

namespace SD_Turizm.Application.Services
{
    public class HotelService : IHotelService
    {
        private readonly IUnitOfWork _unitOfWork;

        public HotelService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Hotel>> GetAllHotelsAsync()
        {
            return await _unitOfWork.Repository<Hotel>().GetAllAsync();
        }

        public async Task<Hotel> GetHotelByIdAsync(int id)
        {
            return await _unitOfWork.Repository<Hotel>().GetByIdAsync(id);
        }

        public async Task<Hotel> GetHotelByCodeAsync(string code)
        {
            var hotels = await _unitOfWork.Repository<Hotel>().FindAsync(h => h.Code == code);
            return hotels.FirstOrDefault();
        }

        public async Task<Hotel> CreateHotelAsync(Hotel hotel)
        {
            await _unitOfWork.Repository<Hotel>().AddAsync(hotel);
            await _unitOfWork.SaveChangesAsync();
            return hotel;
        }

        public async Task UpdateHotelAsync(Hotel hotel)
        {
            await _unitOfWork.Repository<Hotel>().UpdateAsync(hotel);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteHotelAsync(int id)
        {
            await _unitOfWork.Repository<Hotel>().DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<bool> HotelExistsAsync(int id)
        {
            return await _unitOfWork.Repository<Hotel>().ExistsAsync(id);
        }

        public async Task<bool> HotelCodeExistsAsync(string code)
        {
            var hotels = await _unitOfWork.Repository<Hotel>().FindAsync(h => h.Code == code);
            return hotels.Any();
        }
    }
} 