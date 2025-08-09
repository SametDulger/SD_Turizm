using SD_Turizm.Core.Entities;
using SD_Turizm.Core.Interfaces;
using SD_Turizm.Core.DTOs;

namespace SD_Turizm.Application.Services
{
    public class AuditService : IAuditService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AuditService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<AuditLog> LogAsync(string tableName, string action, int recordId, string? userId = null, string? username = null, string? ipAddress = null, string? userAgent = null, string? oldValues = null, string? newValues = null, string? description = null)
        {
            var auditLog = new AuditLog
            {
                TableName = tableName,
                Action = action,
                RecordId = recordId,
                UserId = userId,
                Username = username,
                IpAddress = ipAddress,
                UserAgent = userAgent,
                OldValues = oldValues,
                NewValues = newValues,
                Description = description,
                Timestamp = DateTime.UtcNow,
                CreatedDate = DateTime.UtcNow,
                IsActive = true
            };

            await _unitOfWork.Repository<AuditLog>().AddAsync(auditLog);
            await _unitOfWork.SaveChangesAsync();
            return auditLog;
        }

        public async Task<IEnumerable<AuditLog>> GetAllAsync()
        {
            return await _unitOfWork.Repository<AuditLog>().GetAllAsync();
        }

        public async Task<AuditLog?> GetByIdAsync(int id)
        {
            return await _unitOfWork.Repository<AuditLog>().GetByIdAsync(id);
        }

        public async Task<IEnumerable<AuditLog>> GetByTableNameAsync(string tableName)
        {
            return await _unitOfWork.Repository<AuditLog>().FindAsync(a => a.TableName == tableName);
        }

        public async Task<IEnumerable<AuditLog>> GetByUserIdAsync(string userId)
        {
            return await _unitOfWork.Repository<AuditLog>().FindAsync(a => a.UserId == userId);
        }

        public async Task<IEnumerable<AuditLog>> GetByActionAsync(string action)
        {
            return await _unitOfWork.Repository<AuditLog>().FindAsync(a => a.Action == action);
        }

        public async Task<IEnumerable<AuditLog>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _unitOfWork.Repository<AuditLog>().FindAsync(a => a.Timestamp >= startDate && a.Timestamp <= endDate);
        }

        public async Task<IEnumerable<AuditLog>> GetByRecordIdAsync(string tableName, int recordId)
        {
            return await _unitOfWork.Repository<AuditLog>().FindAsync(a => a.TableName == tableName && a.RecordId == recordId);
        }

        public async Task<PagedResult<AuditLog>> GetPagedAsync(int page, int pageSize, string? searchTerm = null, string? tableName = null, string? action = null, string? userId = null, DateTime? startDate = null, DateTime? endDate = null)
        {
            var query = _unitOfWork.Repository<AuditLog>().GetAllAsync().Result.AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(a => 
                    a.TableName.Contains(searchTerm) || 
                    a.Action.Contains(searchTerm) ||
                    (a.Username != null && a.Username.Contains(searchTerm)) ||
                    (a.Description != null && a.Description.Contains(searchTerm)));
            }

            if (!string.IsNullOrEmpty(tableName))
            {
                query = query.Where(a => a.TableName == tableName);
            }

            if (!string.IsNullOrEmpty(action))
            {
                query = query.Where(a => a.Action == action);
            }

            if (!string.IsNullOrEmpty(userId))
            {
                query = query.Where(a => a.UserId == userId);
            }

            if (startDate.HasValue)
            {
                query = query.Where(a => a.Timestamp >= startDate.Value);
            }

            if (endDate.HasValue)
            {
                query = query.Where(a => a.Timestamp <= endDate.Value);
            }

            // Order by timestamp descending (newest first)
            query = query.OrderByDescending(a => a.Timestamp);

            var totalCount = query.Count();
            var items = query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return await Task.FromResult(new PagedResult<AuditLog>
            {
                Items = items,
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize,
                TotalPages = (int)Math.Ceiling((double)totalCount / pageSize)
            });
        }

        public async Task<IEnumerable<string>> GetAllTableNamesAsync()
        {
            var logs = await GetAllAsync();
            return logs.Select(l => l.TableName).Distinct().OrderBy(t => t);
        }

        public async Task<IEnumerable<string>> GetAllActionsAsync()
        {
            var logs = await GetAllAsync();
            return logs.Select(l => l.Action).Distinct().OrderBy(a => a);
        }

        public async Task<IEnumerable<string>> GetAllUserIdsAsync()
        {
            var logs = await GetAllAsync();
            return logs.Where(l => !string.IsNullOrEmpty(l.UserId))
                      .Select(l => l.UserId!)
                      .Distinct()
                      .OrderBy(u => u);
        }

        public async Task<int> GetTotalLogsCountAsync()
        {
            var logs = await GetAllAsync();
            return logs.Count();
        }

        public async Task<int> GetLogsCountByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            var logs = await GetByDateRangeAsync(startDate, endDate);
            return logs.Count();
        }
    }
}
