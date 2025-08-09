using SD_Turizm.Web.Models.DTOs;

namespace SD_Turizm.Web.Services
{
    public class DashboardApiService : IDashboardApiService
    {
        private readonly IApiClientService _apiClient;

        public DashboardApiService(IApiClientService apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<DashboardStatisticsDto?> GetStatisticsAsync()
        {
            return await _apiClient.GetAsync<DashboardStatisticsDto>("Dashboard/statistics");
        }

        public async Task<List<SalesChartDto>?> GetSalesChartDataAsync()
        {
            return await _apiClient.GetAsync<List<SalesChartDto>>("Dashboard/sales-chart");
        }

        public async Task<List<VendorStatDto>?> GetVendorStatsAsync()
        {
            return await _apiClient.GetAsync<List<VendorStatDto>>("Dashboard/vendor-stats");
        }

        public async Task<List<MonthlyRevenueDto>?> GetMonthlyRevenueAsync()
        {
            return await _apiClient.GetAsync<List<MonthlyRevenueDto>>("Dashboard/monthly-revenue");
        }

        public async Task<List<TopSellingPackageDto>?> GetTopSellingPackagesAsync()
        {
            return await _apiClient.GetAsync<List<TopSellingPackageDto>>("Dashboard/top-packages");
        }

        public async Task<List<RecentActivityDto>?> GetRecentActivitiesAsync()
        {
            return await _apiClient.GetAsync<List<RecentActivityDto>>("Dashboard/recent-activities");
        }
    }
}
