using Microsoft.AspNetCore.Mvc;
using SD_Turizm.Application.Services;

namespace SD_Turizm.API.Controllers.V1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardService _dashboardService;

        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        [HttpGet("stats")]
        public async Task<ActionResult<object>> GetStats(
            [FromQuery] DateTime? startDate = null,
            [FromQuery] DateTime? endDate = null)
        {
            var stats = await _dashboardService.GetStatsAsync(startDate, endDate);
            return Ok(stats);
        }

        [HttpGet("vendor-stats")]
        public async Task<ActionResult<object>> GetVendorStats()
        {
            var vendorStats = await _dashboardService.GetVendorStatsAsync();
            return Ok(vendorStats);
        }

        [HttpGet("statistics")]
        public async Task<ActionResult<object>> GetStatistics()
        {
            var statistics = await _dashboardService.GetStatisticsAsync();
            return Ok(statistics);
        }
    }
}
