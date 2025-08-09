using Microsoft.AspNetCore.Mvc;
using SD_Turizm.Application.Services;
using SD_Turizm.Core.DTOs;
using SD_Turizm.Core.Entities;

namespace SD_Turizm.API.Controllers.V2
{
    [ApiController]
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class CariTransactionController : ControllerBase
    {
        private readonly ICariTransactionService _cariTransactionService;
        private readonly ILoggingService _loggingService;

        public CariTransactionController(ICariTransactionService cariTransactionService, ILoggingService loggingService)
        {
            _cariTransactionService = cariTransactionService;
            _loggingService = loggingService;
        }

        [HttpGet]
        public async Task<ActionResult<PagedResult<CariTransaction>>> GetAll(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20,
            [FromQuery] string? sortBy = "date",
            [FromQuery] string? sortOrder = "desc",
            [FromQuery] string? cariCode = null,
            [FromQuery] DateTime? startDate = null,
            [FromQuery] DateTime? endDate = null,
            [FromQuery] string? transactionType = null,
            [FromQuery] decimal? minAmount = null,
            [FromQuery] decimal? maxAmount = null)
        {
            try
            {
                var paginationDto = new PaginationDto
                {
                    Page = page,
                    PageSize = pageSize
                };

                var result = await _cariTransactionService.GetCariTransactionsWithPaginationAsync(
                    paginationDto, cariCode, transactionType, startDate, endDate, minAmount, maxAmount);

                _loggingService.LogInformation("Cari transactions retrieved with pagination", new { page, pageSize, filters = new { startDate, endDate, transactionType } });

                return Ok(result);
            }
                            catch (Exception ex)
                {
                    _loggingService.LogError("Error retrieving cari transactions with pagination", ex);
                    return StatusCode(500, "Internal server error");
                }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CariTransaction>> GetById(int id)
        {
            var transaction = await _cariTransactionService.GetByIdAsync(id);
            if (transaction == null)
                return NotFound();
            return Ok(transaction);
        }

        [HttpGet("customer/{cariCode}")]
        public async Task<ActionResult<PagedResult<CariTransaction>>> GetByCustomer(
            string cariCode,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20,
            [FromQuery] DateTime? startDate = null,
            [FromQuery] DateTime? endDate = null,
            [FromQuery] string? transactionType = null)
        {
            try
            {
                var paginationDto = new PaginationDto
                {
                    Page = page,
                    PageSize = pageSize
                };

                var result = await _cariTransactionService.GetCustomerTransactionsAsync(
                    paginationDto, cariCode);

                return Ok(result);
            }
                            catch (Exception ex)
                {
                    _loggingService.LogError("Error retrieving customer transactions", ex, new { cariCode });
                    return StatusCode(500, "Internal server error");
                }
        }

        [HttpGet("balance/{cariCode}")]
        public async Task<ActionResult<object>> GetCustomerBalance(string cariCode)
        {
            try
            {
                var balance = await _cariTransactionService.GetCustomerBalanceAsync(cariCode);
                return Ok(new { cariCode, balance });
            }
                            catch (Exception ex)
                {
                    _loggingService.LogError("Error retrieving customer balance", ex, new { cariCode });
                    return StatusCode(500, "Internal server error");
                }
        }

        [HttpGet("stats")]
        public async Task<ActionResult<object>> GetTransactionStats(
            [FromQuery] string period = "monthly",
            [FromQuery] DateTime? startDate = null,
            [FromQuery] DateTime? endDate = null,
            [FromQuery] string? groupBy = "type")
        {
            try
            {
                var stats = await _cariTransactionService.GetTransactionStatisticsAsync();
                return Ok(stats);
            }
                            catch (Exception ex)
                {
                    _loggingService.LogError("Error retrieving transaction statistics", ex);
                    return StatusCode(500, "Internal server error");
                }
        }

        [HttpGet("export")]
        public async Task<IActionResult> ExportTransactions(
            [FromQuery] string format = "excel",
            [FromQuery] string? cariCode = null,
            [FromQuery] DateTime? startDate = null,
            [FromQuery] DateTime? endDate = null,
            [FromQuery] string? transactionType = null)
        {
            try
            {
                var exportData = await _cariTransactionService.ExportTransactionsAsync(format, cariCode, startDate, endDate);
                
                var fileName = $"cari_transactions_{DateTime.Now:yyyyMMdd_HHmmss}.{format}";
                var contentType = format.ToLower() == "excel" ? "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" : "text/csv";
                
                return File(exportData, contentType, fileName);
            }
                            catch (Exception ex)
                {
                    _loggingService.LogError("Error exporting transactions", ex);
                    return StatusCode(500, "Internal server error");
                }
        }

        [HttpPost]
        public async Task<ActionResult<CariTransaction>> Create(CariTransaction transaction)
        {
            try
            {
                var createdTransaction = await _cariTransactionService.CreateAsync(transaction);
                _loggingService.LogInformation("Cari transaction created", new { transactionId = createdTransaction.Id, cariCode = createdTransaction.CariCode });
                return CreatedAtAction(nameof(GetById), new { id = createdTransaction.Id }, createdTransaction);
            }
                            catch (Exception ex)
                {
                    _loggingService.LogError("Error creating cari transaction", ex);
                    return StatusCode(500, "Internal server error");
                }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, CariTransaction transaction)
        {
            if (id != transaction.Id)
                return BadRequest();

            if (!await _cariTransactionService.ExistsAsync(id))
                return NotFound();

            try
            {
                await _cariTransactionService.UpdateAsync(transaction);
                _loggingService.LogInformation("Cari transaction updated", new { transactionId = id });
                return NoContent();
            }
                            catch (Exception ex)
                {
                    _loggingService.LogError("Error updating cari transaction", ex, new { transactionId = id });
                    return StatusCode(500, "Internal server error");
                }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (!await _cariTransactionService.ExistsAsync(id))
                return NotFound();

            try
            {
                await _cariTransactionService.DeleteAsync(id);
                _loggingService.LogInformation("Cari transaction deleted", new { transactionId = id });
                return NoContent();
            }
                            catch (Exception ex)
                {
                    _loggingService.LogError("Error deleting cari transaction", ex, new { transactionId = id });
                    return StatusCode(500, "Internal server error");
                }
        }
    }
}
