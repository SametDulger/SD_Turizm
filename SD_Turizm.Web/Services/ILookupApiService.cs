namespace SD_Turizm.Web.Services
{
    public interface ILookupApiService
    {
        Task<List<dynamic>?> GetCurrenciesAsync();
        Task<List<dynamic>?> GetRoomTypesAsync();
        Task<List<dynamic>?> GetBoardTypesAsync();
        Task<List<dynamic>?> GetVendorTypesAsync();
        Task<List<dynamic>?> GetSaleStatusesAsync();
        Task<List<dynamic>?> GetPersonTypesAsync();
        Task<List<dynamic>?> GetRoomLocationsAsync();
        Task<List<dynamic>?> GetProductTypesAsync();
        Task<List<dynamic>?> GetRolesAsync();
        Task<List<dynamic>?> GetTransactionTypesAsync();
    }
}
