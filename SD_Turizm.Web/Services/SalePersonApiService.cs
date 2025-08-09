using SD_Turizm.Web.Models.DTOs;

namespace SD_Turizm.Web.Services
{
    public class SalePersonApiService : ISalePersonApiService
    {
        private readonly IApiClientService _apiClient;

        public SalePersonApiService(IApiClientService apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<List<SalePersonDto>?> GetAllSalePersonsAsync()
        {
            return await _apiClient.GetAsync<List<SalePersonDto>>("SalePerson");
        }

        public async Task<SalePersonDto?> GetSalePersonByIdAsync(int id)
        {
            return await _apiClient.GetAsync<SalePersonDto>($"SalePerson/{id}");
        }

        public async Task<SalePersonDto?> CreateSalePersonAsync(SalePersonDto salePerson)
        {
            return await _apiClient.PostAsync<SalePersonDto>("SalePerson", salePerson);
        }

        public async Task<SalePersonDto?> UpdateSalePersonAsync(int id, SalePersonDto salePerson)
        {
            return await _apiClient.PutAsync<SalePersonDto>($"SalePerson/{id}", salePerson);
        }

        public async Task<bool> DeleteSalePersonAsync(int id)
        {
            return await _apiClient.DeleteAsync($"SalePerson/{id}");
        }

        public async Task<List<SalePersonDto>?> GetSalePersonsByDepartmentAsync(string department)
        {
            return await _apiClient.GetAsync<List<SalePersonDto>>($"SalePerson/department/{department}");
        }
    }
}
