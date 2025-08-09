using SD_Turizm.Web.Models.DTOs;

namespace SD_Turizm.Web.Services
{
    public interface IAuditApiService
    {
        Task<List<AuditLogDto>?> GetAllAuditLogsAsync();
        Task<AuditLogDto?> GetAuditLogByIdAsync(int id);
        Task<List<AuditLogDto>?> GetAuditLogsByUserAsync(int userId);
        Task<List<AuditLogDto>?> GetAuditLogsByActionAsync(string action);
        Task<List<AuditLogDto>?> GetAuditLogsByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<List<AuditLogDto>?> GetAuditLogsByTableNameAsync(string tableName);
    }
}
