using SD_Turizm.Core.DTOs;

namespace SD_Turizm.Application.Services
{
    public interface IDashboardService
    {
        Task<object> GetStatsAsync(DateTime? startDate = null, DateTime? endDate = null);
        Task<object> GetVendorStatsAsync();
        Task<object> GetStatisticsAsync();
        Task<object> GetSalesChartDataAsync();
        Task<object> GetRecentActivitiesAsync();
        
        // Dashboard Widgets
        Task<object> GetSalesChartWidgetAsync(string period = "month", string chartType = "line", int limit = 10);
        Task<object> GetRevenueGaugeWidgetAsync(string currency = "TRY", string period = "month");
        Task<object> GetTopProductsWidgetAsync(int limit = 5, string sortBy = "revenue", DateTime? startDate = null, DateTime? endDate = null);
        Task<object> GetCustomerActivityWidgetAsync(string period = "week", int limit = 10);
        Task<object> GetLiveStatsAsync();
        Task<object> GetNotificationsAsync(string type = "all", bool unreadOnly = false, int limit = 10);
        Task<bool> MarkNotificationAsReadAsync(int id);
        Task<object> GetUserPreferencesAsync();
        Task<bool> SaveUserPreferencesAsync(object preferences);
        
        // Advanced Analytics
        Task<object> GetSalesAnalyticsAsync(DateTime startDate, DateTime endDate, string groupBy = "day");
        Task<object> GetCustomerAnalyticsAsync(DateTime startDate, DateTime endDate);
        Task<object> GetProductPerformanceAsync(DateTime startDate, DateTime endDate);
        Task<object> GetGeographicAnalyticsAsync(DateTime startDate, DateTime endDate);
        Task<object> GetTrendAnalysisAsync(string metric, DateTime startDate, DateTime endDate);
        Task<object> GetForecastDataAsync(string metric, int periods = 12);
    }
}
