using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using SD_Turizm.Application.Services;
using SD_Turizm.Core.Entities;
using SD_Turizm.Core.DTOs;

namespace SD_Turizm.API.Controllers.V1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [Authorize]
    public class AuditController : ControllerBase
    {
        private readonly IAuditService _auditService;
        private readonly ILoggingService _loggingService;

        public AuditController(IAuditService auditService, ILoggingService loggingService)
        {
            _auditService = auditService;
            _loggingService = loggingService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AuditLog>>> GetAll()
        {
            try
            {
                var auditLogs = await _auditService.GetAllAsync();
                return Ok(auditLogs);
            }
            catch (Exception ex)
            {
                _loggingService.LogError("Error getting all audit logs", ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AuditLog>> GetById(int id)
        {
            try
            {
                var auditLog = await _auditService.GetByIdAsync(id);
                if (auditLog == null)
                    return NotFound();

                return Ok(auditLog);
            }
            catch (Exception ex)
            {
                _loggingService.LogError($"Error getting audit log with id {id}", ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("table/{tableName}")]
        public async Task<ActionResult<IEnumerable<AuditLog>>> GetByTableName(string tableName)
        {
            try
            {
                var auditLogs = await _auditService.GetByTableNameAsync(tableName);
                return Ok(auditLogs);
            }
            catch (Exception ex)
            {
                _loggingService.LogError($"Error getting audit logs for table {tableName}", ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<AuditLog>>> GetByUserId(string userId)
        {
            try
            {
                var auditLogs = await _auditService.GetByUserIdAsync(userId);
                return Ok(auditLogs);
            }
            catch (Exception ex)
            {
                _loggingService.LogError($"Error getting audit logs for user {userId}", ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("action/{action}")]
        public async Task<ActionResult<IEnumerable<AuditLog>>> GetByAction(string action)
        {
            try
            {
                var auditLogs = await _auditService.GetByActionAsync(action);
                return Ok(auditLogs);
            }
            catch (Exception ex)
            {
                _loggingService.LogError($"Error getting audit logs for action {action}", ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("daterange")]
        public async Task<ActionResult<IEnumerable<AuditLog>>> GetByDateRange([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            try
            {
                var auditLogs = await _auditService.GetByDateRangeAsync(startDate, endDate);
                return Ok(auditLogs);
            }
            catch (Exception ex)
            {
                _loggingService.LogError($"Error getting audit logs for date range {startDate} to {endDate}", ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("record/{tableName}/{recordId}")]
        public async Task<ActionResult<IEnumerable<AuditLog>>> GetByRecordId(string tableName, int recordId)
        {
            try
            {
                var auditLogs = await _auditService.GetByRecordIdAsync(tableName, recordId);
                return Ok(auditLogs);
            }
            catch (Exception ex)
            {
                _loggingService.LogError($"Error getting audit logs for record {recordId} in table {tableName}", ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("tables")]
        public async Task<ActionResult<IEnumerable<string>>> GetAllTableNames()
        {
            try
            {
                var tableNames = await _auditService.GetAllTableNamesAsync();
                return Ok(tableNames);
            }
            catch (Exception ex)
            {
                _loggingService.LogError("Error getting all table names", ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("actions")]
        public async Task<ActionResult<IEnumerable<string>>> GetAllActions()
        {
            try
            {
                var actions = await _auditService.GetAllActionsAsync();
                return Ok(actions);
            }
            catch (Exception ex)
            {
                _loggingService.LogError("Error getting all actions", ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("users")]
        public async Task<ActionResult<IEnumerable<string>>> GetAllUserIds()
        {
            try
            {
                var userIds = await _auditService.GetAllUserIdsAsync();
                return Ok(userIds);
            }
            catch (Exception ex)
            {
                _loggingService.LogError("Error getting all user IDs", ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("count")]
        public async Task<ActionResult<int>> GetTotalCount()
        {
            try
            {
                var count = await _auditService.GetTotalLogsCountAsync();
                return Ok(count);
            }
            catch (Exception ex)
            {
                _loggingService.LogError("Error getting total logs count", ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("count/daterange")]
        public async Task<ActionResult<int>> GetCountByDateRange([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            try
            {
                var count = await _auditService.GetLogsCountByDateRangeAsync(startDate, endDate);
                return Ok(count);
            }
            catch (Exception ex)
            {
                _loggingService.LogError($"Error getting logs count for date range {startDate} to {endDate}", ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("paged")]
        public async Task<ActionResult<PagedResult<AuditLog>>> GetPaged(
            [FromQuery] int page = 1, 
            [FromQuery] int pageSize = 10, 
            [FromQuery] string? searchTerm = null,
            [FromQuery] string? tableName = null,
            [FromQuery] string? action = null,
            [FromQuery] string? userId = null,
            [FromQuery] DateTime? startDate = null,
            [FromQuery] DateTime? endDate = null)
        {
            try
            {
                var auditLogs = await _auditService.GetPagedAsync(page, pageSize, searchTerm, tableName, action, userId, startDate, endDate);
                return Ok(auditLogs);
            }
            catch (Exception ex)
            {
                _loggingService.LogError("Error getting paged audit logs", ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("log")]
        public async Task<ActionResult<AuditLog>> Log([FromBody] CreateAuditLogRequest request)
        {
            try
            {
                var auditLog = await _auditService.LogAsync(
                    request.TableName,
                    request.Action,
                    request.RecordId,
                    request.UserId,
                    request.Username,
                    request.IpAddress,
                    request.UserAgent,
                    request.OldValues,
                    request.NewValues,
                    request.Description);

                return CreatedAtAction(nameof(GetById), new { id = auditLog.Id }, auditLog);
            }
            catch (Exception ex)
            {
                _loggingService.LogError("Error creating audit log", ex);
                return StatusCode(500, "Internal server error");
            }
        }
    }

    public class CreateAuditLogRequest
    {
        public string TableName { get; set; } = string.Empty;
        public string Action { get; set; } = string.Empty;
        public int RecordId { get; set; }
        public string? UserId { get; set; }
        public string? Username { get; set; }
        public string? IpAddress { get; set; }
        public string? UserAgent { get; set; }
        public string? OldValues { get; set; }
        public string? NewValues { get; set; }
        public string? Description { get; set; }
    }
}
