using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SD_Turizm.Application.Services;
using SD_Turizm.Core.DTOs;

namespace SD_Turizm.API.Controllers.V2
{
    [ApiController]
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Authorize]
    public class ReportsController : ControllerBase
    {
        private readonly IReportService _reportService;
        private readonly ILoggingService _loggingService;

        public ReportsController(IReportService reportService, ILoggingService loggingService)
        {
            _reportService = reportService;
            _loggingService = loggingService;
        }

        [HttpGet("sales")]
        public async Task<ActionResult<PagedResult<SalesReportDto>>> GetSalesReport(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 50,
            [FromQuery] string? dateRange = null,
            [FromQuery] string? groupBy = null,
            [FromQuery] bool includeDetails = false,
            [FromQuery] DateTime? startDate = null,
            [FromQuery] DateTime? endDate = null)
        {
            try
            {
                var paginationDto = new PaginationDto
                {
                    Page = page,
                    PageSize = pageSize
                };

                var result = await _reportService.GetSalesReportWithPaginationAsync(
                    paginationDto, startDate, endDate);

                _loggingService.LogInformation("Sales report generated", new { page, pageSize, dateRange, groupBy });

                return Ok(result);
            }
            catch (Exception ex)
            {
                _loggingService.LogError("Error generating sales report", ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("sales/export")]
        public async Task<IActionResult> ExportSalesReport(
            [FromQuery] string format = "excel",
            [FromQuery] string? template = "detailed",
            [FromQuery] DateTime? startDate = null,
            [FromQuery] DateTime? endDate = null,
            [FromQuery] string? groupBy = null)
        {
            try
            {
                var exportData = await _reportService.ExportSalesReportAsync(format, startDate, endDate);
                
                var fileName = $"sales_report_{DateTime.Now:yyyyMMdd_HHmmss}.{format}";
                var contentType = format.ToLower() == "excel" ? "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" : "application/pdf";
                
                return File(exportData, contentType, fileName);
            }
            catch (Exception ex)
            {
                _loggingService.LogError("Error exporting sales report", ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("financial")]
        public async Task<ActionResult<PagedResult<FinancialReportDto>>> GetFinancialReport(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 50,
            [FromQuery] string? period = null,
            [FromQuery] string? currency = null,
            [FromQuery] DateTime? startDate = null,
            [FromQuery] DateTime? endDate = null)
        {
            try
            {
                var paginationDto = new PaginationDto
                {
                    Page = page,
                    PageSize = pageSize
                };

                var result = await _reportService.GetFinancialReportWithPaginationAsync(
                    paginationDto, startDate, endDate, currency ?? "TRY");

                return Ok(result);
            }
            catch (Exception ex)
            {
                _loggingService.LogError("Error generating financial report", ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("financial/export")]
        public async Task<IActionResult> ExportFinancialReport(
            [FromQuery] string format = "excel",
            [FromQuery] DateTime? startDate = null,
            [FromQuery] DateTime? endDate = null,
            [FromQuery] string? period = null,
            [FromQuery] string? currency = null)
        {
            try
            {
                var exportData = await _reportService.ExportFinancialReportAsync(format, startDate, endDate, currency ?? "TRY");
                
                var fileName = $"financial_report_{DateTime.Now:yyyyMMdd_HHmmss}.{format}";
                var contentType = format.ToLower() == "excel" ? "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" : "application/pdf";
                
                return File(exportData, contentType, fileName);
            }
            catch (Exception ex)
            {
                _loggingService.LogError("Error exporting financial report", ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("customer")]
        public async Task<ActionResult<PagedResult<CustomerReportDto>>> GetCustomerReport(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 50,
            [FromQuery] DateTime? startDate = null,
            [FromQuery] DateTime? endDate = null,
            [FromQuery] string? cariCode = null,
            [FromQuery] string? customerType = null,
            [FromQuery] string? sortBy = "totalSales",
            [FromQuery] string? sortOrder = "desc")
        {
            try
            {
                var paginationDto = new PaginationDto
                {
                    Page = page,
                    PageSize = pageSize
                };

                var result = await _reportService.GetCustomerReportWithPaginationAsync(
                    paginationDto, startDate, endDate, cariCode);

                return Ok(result);
            }
            catch (Exception ex)
            {
                _loggingService.LogError("Error generating customer report", ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("product")]
        public async Task<ActionResult<PagedResult<ProductReportDto>>> GetProductReport(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 50,
            [FromQuery] string? productType = null,
            [FromQuery] DateTime? startDate = null,
            [FromQuery] DateTime? endDate = null)
        {
            try
            {
                var paginationDto = new PaginationDto
                {
                    Page = page,
                    PageSize = pageSize
                };

                var result = await _reportService.GetProductReportWithPaginationAsync(
                    paginationDto, startDate, endDate, productType);

                return Ok(result);
            }
            catch (Exception ex)
            {
                _loggingService.LogError("Error generating product report", ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("live-sales")]
        public async Task<ActionResult<object>> GetLiveSalesReport()
        {
            try
            {
                var liveData = await _reportService.GetLiveSalesDataAsync();
                return Ok(liveData);
            }
            catch (Exception ex)
            {
                _loggingService.LogError("Error retrieving live sales data", ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("dashboard-widgets")]
        public async Task<ActionResult<object>> GetDashboardWidgets(
            [FromQuery] string? widgetType = null)
        {
            try
            {
                var widgets = await _reportService.GetDashboardWidgetsAsync();
                return Ok(widgets);
            }
            catch (Exception ex)
            {
                _loggingService.LogError("Error retrieving dashboard widgets", ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("summary")]
        public async Task<ActionResult<object>> GetSummary(
            [FromQuery] DateTime? startDate = null,
            [FromQuery] DateTime? endDate = null)
        {
            var summary = await _reportService.GetSummaryAsync(startDate, endDate);
            return Ok(summary);
        }
    }
}
