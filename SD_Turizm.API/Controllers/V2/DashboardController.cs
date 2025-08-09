using Microsoft.AspNetCore.Mvc;
using SD_Turizm.Application.Services;

namespace SD_Turizm.API.Controllers.V2
{
    [ApiController]
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardService _dashboardService;
        private readonly ILoggingService _loggingService;

        public DashboardController(IDashboardService dashboardService, ILoggingService loggingService)
        {
            _dashboardService = dashboardService;
            _loggingService = loggingService;
        }

        [HttpGet("widgets/sales-chart")]
        public async Task<ActionResult<object>> GetSalesChartWidget(
            [FromQuery] string period = "weekly",
            [FromQuery] string? chartType = "line",
            [FromQuery] int? limit = null)
        {
            try
            {
                var chartData = await _dashboardService.GetSalesChartWidgetAsync(period, chartType ?? "line", limit ?? 10);
                return Ok(chartData);
            }
                            catch (Exception ex)
                {
                    _loggingService.LogError("Error retrieving sales chart widget", ex);
                    return StatusCode(500, "Internal server error");
                }
        }

        [HttpGet("widgets/revenue-gauge")]
        public async Task<ActionResult<object>> GetRevenueGaugeWidget(
            [FromQuery] string currency = "USD",
            [FromQuery] string period = "monthly")
        {
            try
            {
                var gaugeData = await _dashboardService.GetRevenueGaugeWidgetAsync(currency, period);
                return Ok(gaugeData);
            }
                            catch (Exception ex)
                {
                    _loggingService.LogError("Error retrieving revenue gauge widget", ex);
                    return StatusCode(500, "Internal server error");
                }
        }

        [HttpGet("widgets/top-products")]
        public async Task<ActionResult<object>> GetTopProductsWidget(
            [FromQuery] int limit = 10,
            [FromQuery] string sortBy = "revenue",
            [FromQuery] DateTime? startDate = null,
            [FromQuery] DateTime? endDate = null)
        {
            try
            {
                var topProducts = await _dashboardService.GetTopProductsWidgetAsync(limit, sortBy, startDate, endDate);
                return Ok(topProducts);
            }
                            catch (Exception ex)
                {
                    _loggingService.LogError("Error retrieving top products widget", ex);
                    return StatusCode(500, "Internal server error");
                }
        }

        [HttpGet("widgets/customer-activity")]
        public async Task<ActionResult<object>> GetCustomerActivityWidget(
            [FromQuery] string period = "daily",
            [FromQuery] int limit = 20)
        {
            try
            {
                var activityData = await _dashboardService.GetCustomerActivityWidgetAsync(period, limit);
                return Ok(activityData);
            }
                            catch (Exception ex)
                {
                    _loggingService.LogError("Error retrieving customer activity widget", ex);
                    return StatusCode(500, "Internal server error");
                }
        }

        [HttpGet("live-stats")]
        public async Task<ActionResult<object>> GetLiveStats()
        {
            try
            {
                var liveStats = await _dashboardService.GetLiveStatsAsync();
                return Ok(liveStats);
            }
                            catch (Exception ex)
                {
                    _loggingService.LogError("Error retrieving live stats", ex);
                    return StatusCode(500, "Internal server error");
                }
        }

        [HttpGet("notifications")]
        public async Task<ActionResult<object>> GetNotifications(
            [FromQuery] string? type = null,
            [FromQuery] bool unreadOnly = true,
            [FromQuery] int limit = 50)
        {
            try
            {
                var notifications = await _dashboardService.GetNotificationsAsync(type ?? "all", unreadOnly, limit);
                return Ok(notifications);
            }
                            catch (Exception ex)
                {
                    _loggingService.LogError("Error retrieving notifications", ex);
                    return StatusCode(500, "Internal server error");
                }
        }

        [HttpPost("notifications/{id}/mark-read")]
        public async Task<IActionResult> MarkNotificationAsRead(int id)
        {
            try
            {
                await _dashboardService.MarkNotificationAsReadAsync(id);
                return NoContent();
            }
                            catch (Exception ex)
                {
                    _loggingService.LogError("Error marking notification as read", ex, new { notificationId = id });
                    return StatusCode(500, "Internal server error");
                }
        }

        [HttpGet("user-preferences")]
        public async Task<ActionResult<object>> GetUserPreferences()
        {
            try
            {
                var preferences = await _dashboardService.GetUserPreferencesAsync();
                return Ok(preferences);
            }
                            catch (Exception ex)
                {
                    _loggingService.LogError("Error retrieving user preferences", ex);
                    return StatusCode(500, "Internal server error");
                }
        }

        [HttpPost("save-preferences")]
        public async Task<IActionResult> SaveUserPreferences([FromBody] object preferences)
        {
            try
            {
                await _dashboardService.SaveUserPreferencesAsync(preferences);
                return NoContent();
            }
                            catch (Exception ex)
                {
                    _loggingService.LogError("Error saving user preferences", ex);
                    return StatusCode(500, "Internal server error");
                }
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
            try
            {
                var vendorStats = await _dashboardService.GetVendorStatsAsync();
                return Ok(vendorStats);
            }
            catch (Exception ex)
            {
                _loggingService.LogError("Error retrieving vendor stats", ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("statistics")]
        public async Task<ActionResult<object>> GetStatistics()
        {
            try
            {
                var statistics = await _dashboardService.GetStatisticsAsync();
                return Ok(statistics);
            }
            catch (Exception ex)
            {
                _loggingService.LogError("Error retrieving statistics", ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("sales-chart")]
        public async Task<ActionResult<object>> GetSalesChart()
        {
            try
            {
                var chartData = await _dashboardService.GetSalesChartDataAsync();
                return Ok(chartData);
            }
            catch (Exception ex)
            {
                _loggingService.LogError("Error retrieving sales chart data", ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("recent-activities")]
        public async Task<ActionResult<object>> GetRecentActivities()
        {
            try
            {
                var activities = await _dashboardService.GetRecentActivitiesAsync();
                return Ok(activities);
            }
            catch (Exception ex)
            {
                _loggingService.LogError("Error retrieving recent activities", ex);
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
