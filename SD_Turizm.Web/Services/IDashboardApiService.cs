using SD_Turizm.Web.Models.DTOs;

namespace SD_Turizm.Web.Services
{
    public interface IDashboardApiService
    {
        Task<DashboardStatisticsDto?> GetStatisticsAsync();
        Task<List<SalesChartDto>?> GetSalesChartDataAsync();
        Task<List<VendorStatDto>?> GetVendorStatsAsync();
        Task<List<MonthlyRevenueDto>?> GetMonthlyRevenueAsync();
        Task<List<TopSellingPackageDto>?> GetTopSellingPackagesAsync();
        Task<List<RecentActivityDto>?> GetRecentActivitiesAsync();
    }

    public class DashboardStatisticsDto
    {
        public int TotalSales { get; set; }
        public decimal TotalRevenue { get; set; }
        public int ActiveVendors { get; set; }
        public int TotalHotels { get; set; }
        public int TotalTours { get; set; }
        public int TotalCustomers { get; set; }
        public int TotalPackages { get; set; }
        public decimal MonthlyRevenue { get; set; }
        public int MonthlySales { get; set; }
    }

    public class SalesChartDto
    {
        public string Label { get; set; } = string.Empty;
        public decimal Value { get; set; }
        public DateTime Date { get; set; }
    }

    public class VendorStatDto
    {
        public string VendorName { get; set; } = string.Empty;
        public int TotalSales { get; set; }
        public decimal TotalRevenue { get; set; }
    }

    public class MonthlyRevenueDto
    {
        public string Month { get; set; } = string.Empty;
        public decimal Revenue { get; set; }
        public int SalesCount { get; set; }
    }

    public class TopSellingPackageDto
    {
        public string PackageName { get; set; } = string.Empty;
        public int SalesCount { get; set; }
        public decimal Revenue { get; set; }
    }

    public class RecentActivityDto
    {
        public string Activity { get; set; } = string.Empty;
        public string User { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; }
        public string Type { get; set; } = string.Empty;
    }
}
