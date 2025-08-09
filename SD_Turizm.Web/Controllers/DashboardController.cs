using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using SD_Turizm.Web.Models.DTOs;
using SD_Turizm.Web.Services;
using System.Text.Json;

namespace SD_Turizm.Web.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly IDashboardApiService _dashboardApiService;
        private readonly IHotelApiService _hotelApiService;
        private readonly ITourApiService _tourApiService;
        private readonly ISaleApiService _saleApiService;

        public DashboardController(IDashboardApiService dashboardApiService, IHotelApiService hotelApiService, 
            ITourApiService tourApiService, ISaleApiService saleApiService)
        {
            _dashboardApiService = dashboardApiService;
            _hotelApiService = hotelApiService;
            _tourApiService = tourApiService;
            _saleApiService = saleApiService;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var dashboardStats = await _dashboardApiService.GetStatisticsAsync();
                
                if (dashboardStats != null)
                {
                    var dashboardData = new DashboardViewModel
                    {
                        TotalSales = dashboardStats.TotalSales,
                        TotalRevenue = dashboardStats.TotalRevenue,
                        ActiveVendors = dashboardStats.ActiveVendors,
                        TotalHotels = dashboardStats.TotalHotels,
                        TotalTours = dashboardStats.TotalTours
                    };
                    
                    return View(dashboardData);
                }
                
                return View(new DashboardViewModel());
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Dashboard yüklenirken hata oluştu.");
                return View(new DashboardViewModel());
            }
        }

        public async Task<IActionResult> SalesChart()
        {
            try
            {
                var chartData = await _dashboardApiService.GetSalesChartDataAsync();
                return Json(chartData ?? new List<SalesChartDto>());
            }
            catch (Exception)
            {
                return Json(new List<SalesChartDto>());
            }
        }

        public async Task<IActionResult> VendorStats()
        {
            try
            {
                var stats = await _dashboardApiService.GetVendorStatsAsync();
                return Json(stats ?? new List<VendorStatDto>());
            }
            catch (Exception)
            {
                return Json(new List<VendorStatDto>());
            }
        }

        public async Task<IActionResult> GetRecentActivities()
        {
            try
            {
                var activities = await _dashboardApiService.GetRecentActivitiesAsync();
                return Json(activities ?? new List<RecentActivityDto>());
            }
            catch (Exception)
            {
                return Json(new List<RecentActivityDto>());
            }
        }
    }

    public class DashboardViewModel
    {
        public int TotalSales { get; set; }
        public decimal TotalRevenue { get; set; }
        public int ActiveVendors { get; set; }
        public int TotalHotels { get; set; }
        public int TotalTours { get; set; }
    }
}
