using Microsoft.AspNetCore.Mvc;
using SD_Turizm.Application.Services;
using SD_Turizm.Core.Entities;

namespace SD_Turizm.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReportsController : ControllerBase
    {
        private readonly IReportService _reportService;

        public ReportsController(IReportService reportService)
        {
            _reportService = reportService;
        }

        [HttpGet("sales")]
        public async Task<ActionResult<IEnumerable<Sale>>> GetSalesReport(
            [FromQuery] DateTime startDate,
            [FromQuery] DateTime endDate,
            [FromQuery] string? sellerType = null,
            [FromQuery] string? currency = null,
            [FromQuery] string? pnrNumber = null,
            [FromQuery] string? fileCode = null,
            [FromQuery] string? agencyCode = null,
            [FromQuery] string? cariCode = null)
        {
            var sales = await _reportService.GetSalesReportAsync(startDate, endDate, sellerType, currency, pnrNumber, fileCode, agencyCode, cariCode);
            return Ok(sales);
        }

        [HttpGet("sales/summary")]
        public async Task<ActionResult<object>> GetSalesSummary(
            [FromQuery] DateTime startDate,
            [FromQuery] DateTime endDate,
            [FromQuery] string? sellerType = null,
            [FromQuery] string? currency = null,
            [FromQuery] string? pnrNumber = null,
            [FromQuery] string? fileCode = null,
            [FromQuery] string? agencyCode = null,
            [FromQuery] string? cariCode = null)
        {
            var summary = await _reportService.GetSalesSummaryAsync(startDate, endDate, sellerType, currency, pnrNumber, fileCode, agencyCode, cariCode);
            return Ok(summary);
        }

        [HttpGet("financial")]
        public async Task<ActionResult<IEnumerable<Sale>>> GetFinancialReport(
            [FromQuery] DateTime startDate,
            [FromQuery] DateTime endDate,
            [FromQuery] string currency = "TRY")
        {
            var sales = await _reportService.GetFinancialReportAsync(startDate, endDate, currency);
            return Ok(sales);
        }

        [HttpGet("financial/summary")]
        public async Task<ActionResult<object>> GetFinancialSummary(
            [FromQuery] DateTime startDate,
            [FromQuery] DateTime endDate,
            [FromQuery] string currency = "TRY")
        {
            var summary = await _reportService.GetFinancialSummaryAsync(startDate, endDate, currency);
            return Ok(summary);
        }

        [HttpGet("customers")]
        public async Task<ActionResult<IEnumerable<Sale>>> GetCustomerReport(
            [FromQuery] DateTime startDate,
            [FromQuery] DateTime endDate,
            [FromQuery] string? cariCode = null)
        {
            var sales = await _reportService.GetCustomerReportAsync(startDate, endDate, cariCode);
            return Ok(sales);
        }

        [HttpGet("customers/summary")]
        public async Task<ActionResult<object>> GetCustomerSummary(
            [FromQuery] DateTime startDate,
            [FromQuery] DateTime endDate,
            [FromQuery] string? cariCode = null)
        {
            var summary = await _reportService.GetCustomerSummaryAsync(startDate, endDate, cariCode);
            return Ok(summary);
        }

        [HttpGet("products")]
        public async Task<ActionResult<IEnumerable<object>>> GetProductReport(
            [FromQuery] DateTime startDate,
            [FromQuery] DateTime endDate,
            [FromQuery] string? productType = null)
        {
            var products = await _reportService.GetProductReportAsync(startDate, endDate, productType);
            return Ok(products);
        }

        [HttpGet("products/summary")]
        public async Task<ActionResult<object>> GetProductSummary(
            [FromQuery] DateTime startDate,
            [FromQuery] DateTime endDate,
            [FromQuery] string? productType = null)
        {
            var summary = await _reportService.GetProductSummaryAsync(startDate, endDate, productType);
            return Ok(summary);
        }
    }
} 