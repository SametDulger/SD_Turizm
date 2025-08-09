using Microsoft.AspNetCore.Mvc;
using SD_Turizm.Application.Services;
using SD_Turizm.Core.DTOs;
using SD_Turizm.Core.Entities;

namespace SD_Turizm.API.Controllers.V2
{
    [ApiController]
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class SalesController : ControllerBase
    {
        private readonly ISaleService _saleService;
        private readonly ILoggingService _loggingService;

        public SalesController(ISaleService saleService, ILoggingService loggingService)
        {
            _saleService = saleService;
            _loggingService = loggingService;
        }

        [HttpGet]
        public async Task<ActionResult<PagedResult<Sale>>> GetAll(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20,
            [FromQuery] string? sortBy = "date",
            [FromQuery] string? sortOrder = "desc",
            [FromQuery] DateTime? startDate = null,
            [FromQuery] DateTime? endDate = null,
            [FromQuery] string? pnr = null,
            [FromQuery] string? agency = null,
            [FromQuery] string? cari = null,
            [FromQuery] decimal? minAmount = null,
            [FromQuery] decimal? maxAmount = null,
            [FromQuery] string? status = null)
        {
            try
            {
                var paginationDto = new PaginationDto
                {
                    Page = page,
                    PageSize = pageSize
                };

                var result = await _saleService.GetSalesWithPaginationAsync(
                    paginationDto, sortBy, sortOrder, startDate, endDate, 
                    pnr, agency, cari, minAmount, maxAmount, status);

                _loggingService.LogInformation("Sales retrieved with pagination", new { page, pageSize, filters = new { startDate, endDate, pnr, agency, cari } });

                return Ok(result);
            }
                            catch (Exception ex)
                {
                    _loggingService.LogError("Error retrieving sales with pagination", ex);
                    return StatusCode(500, "Internal server error");
                }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Sale>> GetById(int id)
        {
            var sale = await _saleService.GetByIdAsync(id);
            if (sale == null)
                return NotFound();
            return Ok(sale);
        }

        [HttpGet("stats")]
        public async Task<ActionResult<object>> GetSalesStats(
            [FromQuery] string period = "monthly",
            [FromQuery] DateTime? startDate = null,
            [FromQuery] DateTime? endDate = null)
        {
            try
            {
                var stats = await _saleService.GetSalesStatisticsAsync(period, startDate, endDate);
                return Ok(stats);
            }
                            catch (Exception ex)
                {
                    _loggingService.LogError("Error retrieving sales statistics", ex);
                    return StatusCode(500, "Internal server error");
                }
        }

        [HttpGet("revenue-chart")]
        public async Task<ActionResult<object>> GetRevenueChart(
            [FromQuery] string groupBy = "product",
            [FromQuery] DateTime? startDate = null,
            [FromQuery] DateTime? endDate = null)
        {
            try
            {
                var chartData = await _saleService.GetRevenueChartDataAsync(groupBy, startDate, endDate);
                return Ok(chartData);
            }
                            catch (Exception ex)
                {
                    _loggingService.LogError("Error retrieving revenue chart data", ex);
                    return StatusCode(500, "Internal server error");
                }
        }

        [HttpGet("export")]
        public async Task<IActionResult> ExportSales(
            [FromQuery] string format = "excel",
            [FromQuery] DateTime? startDate = null,
            [FromQuery] DateTime? endDate = null,
            [FromQuery] string? pnr = null,
            [FromQuery] string? agency = null,
            [FromQuery] string? cari = null)
        {
            try
            {
                var exportData = await _saleService.ExportSalesAsync(format, startDate, endDate, pnr, agency, cari);
                
                var fileName = $"sales_export_{DateTime.Now:yyyyMMdd_HHmmss}.{format}";
                var contentType = format.ToLower() == "excel" ? "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" : "text/csv";
                
                return File(exportData, contentType, fileName);
            }
                            catch (Exception ex)
                {
                    _loggingService.LogError("Error exporting sales data", ex);
                    return StatusCode(500, "Internal server error");
                }
        }

        [HttpPost]
        public async Task<ActionResult<Sale>> Create(Sale sale)
        {
            try
            {
                var createdSale = await _saleService.CreateAsync(sale);
                _loggingService.LogInformation("Sale created", new { saleId = createdSale.Id, pnr = createdSale.PNRNumber });
                return CreatedAtAction(nameof(GetById), new { id = createdSale.Id }, createdSale);
            }
                            catch (Exception ex)
                {
                    _loggingService.LogError("Error creating sale", ex);
                    return StatusCode(500, "Internal server error");
                }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Sale sale)
        {
            if (id != sale.Id)
                return BadRequest();

            if (!await _saleService.ExistsAsync(id))
                return NotFound();

            try
            {
                await _saleService.UpdateAsync(sale);
                _loggingService.LogInformation("Sale updated", new { saleId = id });
                return NoContent();
            }
            catch (Exception ex)
            {
                _loggingService.LogError("Error updating sale", ex, new { saleId = id });
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (!await _saleService.ExistsAsync(id))
                return NotFound();

            try
            {
                await _saleService.DeleteAsync(id);
                _loggingService.LogInformation("Sale deleted", new { saleId = id });
                return NoContent();
            }
            catch (Exception ex)
            {
                _loggingService.LogError("Error deleting sale", ex, new { saleId = id });
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
