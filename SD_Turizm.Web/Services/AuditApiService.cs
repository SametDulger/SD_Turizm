using SD_Turizm.Web.Models.DTOs;

namespace SD_Turizm.Web.Services
{
    public class AuditApiService : IAuditApiService
    {
        private readonly IApiClientService _apiClient;

        public AuditApiService(IApiClientService apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<List<AuditLogDto>?> GetAllAuditLogsAsync()
        {
            return await _apiClient.GetAsync<List<AuditLogDto>>("Audit");
        }

        public async Task<AuditLogDto?> GetAuditLogByIdAsync(int id)
        {
            return await _apiClient.GetAsync<AuditLogDto>($"Audit/{id}");
        }

        public async Task<List<AuditLogDto>?> GetAuditLogsByUserAsync(int userId)
        {
            return await _apiClient.GetAsync<List<AuditLogDto>>($"Audit/user/{userId}");
        }

        public async Task<List<AuditLogDto>?> GetAuditLogsByActionAsync(string action)
        {
            return await _apiClient.GetAsync<List<AuditLogDto>>($"Audit/action/{action}");
        }

        public async Task<List<AuditLogDto>?> GetAuditLogsByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _apiClient.GetAsync<List<AuditLogDto>>($"Audit/daterange?start={startDate:yyyy-MM-dd}&end={endDate:yyyy-MM-dd}");
        }

        public async Task<List<AuditLogDto>?> GetAuditLogsByTableNameAsync(string tableName)
        {
            return await _apiClient.GetAsync<List<AuditLogDto>>($"Audit/table/{tableName}");
        }
    }
}
