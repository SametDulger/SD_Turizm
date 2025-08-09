using SD_Turizm.Web.Models.DTOs;

namespace SD_Turizm.Web.Services
{
    public class AddressApiService : IAddressApiService
    {
        private readonly IApiClientService _apiClient;

        public AddressApiService(IApiClientService apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<List<AddressDto>?> GetAllAddressesAsync()
        {
            return await _apiClient.GetAsync<List<AddressDto>>("Address");
        }

        public async Task<AddressDto?> GetAddressByIdAsync(int id)
        {
            return await _apiClient.GetAsync<AddressDto>($"Address/{id}");
        }

        public async Task<AddressDto?> CreateAddressAsync(AddressDto address)
        {
            return await _apiClient.PostAsync<AddressDto>("Address", address);
        }

        public async Task<AddressDto?> UpdateAddressAsync(int id, AddressDto address)
        {
            return await _apiClient.PutAsync<AddressDto>($"Address/{id}", address);
        }

        public async Task<bool> DeleteAddressAsync(int id)
        {
            return await _apiClient.DeleteAsync($"Address/{id}");
        }

        public async Task<List<AddressDto>?> GetAddressesByCityAsync(string city)
        {
            return await _apiClient.GetAsync<List<AddressDto>>($"Address/city/{city}");
        }

        public async Task<List<AddressDto>?> GetAddressesByCountryAsync(string country)
        {
            return await _apiClient.GetAsync<List<AddressDto>>($"Address/country/{country}");
        }
    }
}
