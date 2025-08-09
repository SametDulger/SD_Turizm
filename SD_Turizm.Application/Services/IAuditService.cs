using SD_Turizm.Core.Entities;
using SD_Turizm.Core.DTOs;

namespace SD_Turizm.Application.Services
{
    public interface IAuditService
    {
        Task<AuditLog> LogAsync(string tableName, string action, int recordId, string? userId = null, string? username = null, string? ipAddress = null, string? userAgent = null, string? oldValues = null, string? newValues = null, string? description = null);
        Task<IEnumerable<AuditLog>> GetAllAsync();
        Task<AuditLog?> GetByIdAsync(int id);
        Task<IEnumerable<AuditLog>> GetByTableNameAsync(string tableName);
        Task<IEnumerable<AuditLog>> GetByUserIdAsync(string userId);
        Task<IEnumerable<AuditLog>> GetByActionAsync(string action);
        Task<IEnumerable<AuditLog>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<IEnumerable<AuditLog>> GetByRecordIdAsync(string tableName, int recordId);
        Task<PagedResult<AuditLog>> GetPagedAsync(int page, int pageSize, string? searchTerm = null, string? tableName = null, string? action = null, string? userId = null, DateTime? startDate = null, DateTime? endDate = null);
        Task<IEnumerable<string>> GetAllTableNamesAsync();
        Task<IEnumerable<string>> GetAllActionsAsync();
        Task<IEnumerable<string>> GetAllUserIdsAsync();
        Task<int> GetTotalLogsCountAsync();
        Task<int> GetLogsCountByDateRangeAsync(DateTime startDate, DateTime endDate);
    }
}
