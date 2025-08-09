using SD_Turizm.Web.Models.DTOs;

namespace SD_Turizm.Web.Services
{
    public interface IAddressApiService
    {
        Task<List<AddressDto>?> GetAllAddressesAsync();
        Task<AddressDto?> GetAddressByIdAsync(int id);
        Task<AddressDto?> CreateAddressAsync(AddressDto address);
        Task<AddressDto?> UpdateAddressAsync(int id, AddressDto address);
        Task<bool> DeleteAddressAsync(int id);
        Task<List<AddressDto>?> GetAddressesByCityAsync(string city);
        Task<List<AddressDto>?> GetAddressesByCountryAsync(string country);
    }
}
