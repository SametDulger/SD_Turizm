using SD_Turizm.Web.Models.DTOs;

namespace SD_Turizm.Web.Services
{
    public interface ISalePersonApiService
    {
        Task<List<SalePersonDto>?> GetAllSalePersonsAsync();
        Task<SalePersonDto?> GetSalePersonByIdAsync(int id);
        Task<SalePersonDto?> CreateSalePersonAsync(SalePersonDto salePerson);
        Task<SalePersonDto?> UpdateSalePersonAsync(int id, SalePersonDto salePerson);
        Task<bool> DeleteSalePersonAsync(int id);
        Task<List<SalePersonDto>?> GetSalePersonsByDepartmentAsync(string department);
    }
}
