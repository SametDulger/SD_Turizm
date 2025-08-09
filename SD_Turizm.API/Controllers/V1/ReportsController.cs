using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SD_Turizm.Application.Services;

namespace SD_Turizm.API.Controllers.V1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Authorize]
    public class ReportsController : ControllerBase
    {
        private readonly IReportService _reportService;

        public ReportsController(IReportService reportService)
        {
            _reportService = reportService;
        }

        [HttpGet("sales")]
        public async Task<ActionResult<IEnumerable<object>>> GetSalesReport(
            [FromQuery] DateTime? startDate = null,
            [FromQuery] DateTime? endDate = null)
        {
            var report = await _reportService.GetSalesReportAsync(startDate ?? DateTime.MinValue, endDate ?? DateTime.MaxValue);
            return Ok(report);
        }

        [HttpGet("financial")]
        public async Task<ActionResult<IEnumerable<object>>> GetFinancialReport(
            [FromQuery] DateTime? startDate = null,
            [FromQuery] DateTime? endDate = null)
        {
            var report = await _reportService.GetFinancialReportAsync(startDate ?? DateTime.MinValue, endDate ?? DateTime.MaxValue);
            return Ok(report);
        }

        [HttpGet("customer")]
        public async Task<ActionResult<IEnumerable<object>>> GetCustomerReport(
            [FromQuery] DateTime? startDate = null,
            [FromQuery] DateTime? endDate = null)
        {
            var report = await _reportService.GetCustomerReportAsync(startDate ?? DateTime.MinValue, endDate ?? DateTime.MaxValue);
            return Ok(report);
        }

        [HttpGet("product")]
        public async Task<ActionResult<IEnumerable<object>>> GetProductReport(
            [FromQuery] DateTime? startDate = null,
            [FromQuery] DateTime? endDate = null)
        {
            var report = await _reportService.GetProductReportAsync(startDate ?? DateTime.MinValue, endDate ?? DateTime.MaxValue);
            return Ok(report);
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
