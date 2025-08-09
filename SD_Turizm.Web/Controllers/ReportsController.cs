using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using SD_Turizm.Web.Models.DTOs;
using SD_Turizm.Web.Services;
using System.Text.Json;

namespace SD_Turizm.Web.Controllers
{
    [Authorize]
    public class ReportsController : Controller
    {
        private readonly IReportsApiService _reportsApiService;
        private readonly ILookupApiService _lookupApiService;

        public ReportsController(IReportsApiService reportsApiService, ILookupApiService lookupApiService)
        {
            _reportsApiService = reportsApiService;
            _lookupApiService = lookupApiService;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                // Dashboard için özet veriler hesapla
                var salesReport = await _reportsApiService.GetSalesReportDataAsync() ?? new { };
                var financialReport = await _reportsApiService.GetFinancialReportDataAsync() ?? new { };
                var customerReport = await _reportsApiService.GetCustomerReportDataAsync() ?? new { };
                var productReport = await _reportsApiService.GetProductReportDataAsync() ?? new { };

                // Sales data
                var salesData = (dynamic)salesReport;
                var sales = ((IEnumerable<dynamic>)(salesData.Sales ?? new List<dynamic>())).ToList();
                
                var monthlySales = sales.Sum(s => (decimal)(s.SalePrice ?? 0));
                var weeklySales = sales.Where(s => 
                    DateTime.TryParse(s.CreatedDate?.ToString(), out DateTime createdDate) && 
                    createdDate >= DateTime.Now.AddDays(-7))
                    .Sum(s => (decimal)(s.SalePrice ?? 0));

                // Financial data
                var financialData = (dynamic)financialReport;
                var financial = ((IEnumerable<dynamic>)(financialData.Financial ?? new List<dynamic>())).ToList();
                
                var totalSaleAmount = financial.Sum(f => (decimal)(f.SalePrice ?? 0));
                var totalCost = financial.Sum(f => (decimal)(f.PurchasePrice ?? 0));
                var totalProfit = totalSaleAmount - totalCost;
                var profitMargin = totalSaleAmount > 0 ? (totalProfit / totalSaleAmount) * 100 : 0;

                // Customer data
                var customerData = (dynamic)customerReport;
                var customers = ((IEnumerable<dynamic>)(customerData.Customers ?? new List<dynamic>())).ToList();
                var totalCustomers = customers.Count;
                var activeCustomers = customers.Count(c => (int)(c.TotalOrders ?? 0) > 0);

                // Product data
                var productData = (dynamic)productReport;
                var products = ((IEnumerable<dynamic>)(productData.Products ?? new List<dynamic>())).ToList();
                var activePackages = products.Count;
                var topSellingProducts = products.Count(p => (int)(p.TotalSales ?? 0) >= 1);

                ViewBag.MonthlySales = monthlySales;
                ViewBag.WeeklySales = weeklySales;
                ViewBag.ProfitMargin = profitMargin;
                ViewBag.MonthlyProfit = totalProfit;
                ViewBag.TotalCustomers = totalCustomers;
                ViewBag.ActiveCustomers = activeCustomers;
                ViewBag.ActivePackages = activePackages;
                ViewBag.TopSellingProducts = topSellingProducts;
            }
            catch (Exception ex)
            {
                // Hata durumunda varsayılan değerler
                ViewBag.MonthlySales = 0m;
                ViewBag.WeeklySales = 0m;
                ViewBag.ProfitMargin = 0.0;
                ViewBag.MonthlyProfit = 0m;
                ViewBag.TotalCustomers = 0;
                ViewBag.ActiveCustomers = 0;
                ViewBag.ActivePackages = 0;
                ViewBag.TopSellingProducts = 0;
            }

            return View();
        }

        public async Task<IActionResult> CustomerReport()
        {
            await LoadLookupData();
            var report = await _reportsApiService.GetCustomerReportDataAsync() ?? new { };
            
            // View'da ViewBag.Customers kullanıldığı için verileri ViewBag'e set ediyoruz
            try
            {
                var customerData = (dynamic)report;
                var customers = customerData.Customers ?? new List<dynamic>();
                ViewBag.Customers = customers;
                
                // Summary hesapla
                var customerList = ((IEnumerable<dynamic>)customers).ToList();
                
                var totalCustomers = customerList.Count;
                var activeCustomers = customerList.Count(c => (int)(c.TotalOrders ?? 0) > 0);
                var totalRevenue = customerList.Sum(c => (decimal)(c.TotalPurchases ?? 0));
                var totalOrders = customerList.Sum(c => (int)(c.TotalOrders ?? 0));
                
                ViewBag.Summary = new {
                    TotalCustomers = totalCustomers,
                    ActiveCustomers = activeCustomers,
                    ActiveRate = totalCustomers > 0 ? (double)activeCustomers / totalCustomers * 100 : 0,
                    PassiveCustomers = totalCustomers - activeCustomers,
                    TotalRevenue = totalRevenue,
                    TotalCustomerValue = totalRevenue, // View'da TotalCustomerValue kullanılıyor
                    AverageCustomerValue = totalCustomers > 0 ? totalRevenue / totalCustomers : 0, // View'da AverageCustomerValue kullanılıyor
                    AverageOrderValue = activeCustomers > 0 ? totalRevenue / activeCustomers : 0,
                    AverageOrders = totalCustomers > 0 ? (double)totalOrders / totalCustomers : 0
                };
            }
            catch (Exception)
            {
                ViewBag.Customers = new List<dynamic>();
                ViewBag.Summary = new {
                    TotalCustomers = 0,
                    ActiveCustomers = 0,
                    ActiveRate = 0.0,
                    PassiveCustomers = 0,
                    TotalRevenue = 0m,
                    TotalCustomerValue = 0m,
                    AverageCustomerValue = 0m,
                    AverageOrderValue = 0m,
                    AverageOrders = 0.0
                };
            }
            
            return View(report);
        }

        public async Task<IActionResult> FinancialReport()
        {
            await LoadLookupData();
            var report = await _reportsApiService.GetFinancialReportDataAsync() ?? new { };
            
            // View'da ViewBag.Financial ve ViewBag.Summary kullanıldığı için verileri set ediyoruz
            try
            {
                var financialData = (dynamic)report;
                var financial = financialData.Financial ?? new List<dynamic>();
                ViewBag.Financial = financial;
                
                // Summary hesapla
                var financialList = ((IEnumerable<dynamic>)financial).ToList();
                
                var totalSales = financialList.Count;
                var totalSaleAmount = financialList.Sum(f => (decimal)(f.SalePrice ?? 0));
                var totalPurchaseAmount = financialList.Sum(f => (decimal)(f.PurchasePrice ?? 0));
                var totalProfit = totalSaleAmount - totalPurchaseAmount;
                
                ViewBag.Summary = new {
                    TotalSales = totalSales,
                    TotalSaleAmount = totalSaleAmount,
                    TotalPurchaseAmount = totalPurchaseAmount,
                    TotalProfit = totalProfit,
                    AverageProfit = totalSales > 0 ? totalProfit / totalSales : 0,
                    ProfitMargin = totalSaleAmount > 0 ? (totalProfit / totalSaleAmount) * 100 : 0
                };
            }
            catch
            {
                ViewBag.Financial = new List<dynamic>();
                ViewBag.Summary = new {
                    TotalSales = 0,
                    TotalSaleAmount = 0m,
                    TotalPurchaseAmount = 0m,
                    TotalProfit = 0m,
                    AverageProfit = 0m,
                    ProfitMargin = 0.0
                };
            }
            
            return View(report);
        }

        public async Task<IActionResult> SalesReport()
        {
            await LoadLookupData();
            var report = await _reportsApiService.GetSalesReportDataAsync() ?? new { };
            
            // View'da ViewBag.Sales kullanıldığı için verileri ViewBag'e set ediyoruz
            try
            {
                var salesData = (dynamic)report;
                ViewBag.Sales = salesData.Sales ?? new List<dynamic>();
            }
            catch
            {
                ViewBag.Sales = new List<dynamic>();
            }
            
            return View(report);
        }

        public async Task<IActionResult> ProductReport()
        {
            await LoadLookupData();
            var report = await _reportsApiService.GetProductReportDataAsync() ?? new { };
            
            // View'da ViewBag.Products kullanıldığı için verileri ViewBag'e set ediyoruz
            try
            {
                var productData = (dynamic)report;
                var products = productData.Products ?? new List<dynamic>();
                ViewBag.Products = products;
                
                // Summary hesapla
                var productList = ((IEnumerable<dynamic>)products).ToList();
                
                var hotelProducts = productList.Where(p => p.ProductType?.ToString().ToLower().Contains("otel") == true);
                var tourProducts = productList.Where(p => p.ProductType?.ToString().ToLower().Contains("tur") == true);
                var flightProducts = productList.Where(p => p.ProductType?.ToString().ToLower().Contains("uçak") == true);
                var cruiseProducts = productList.Where(p => p.ProductType?.ToString().ToLower().Contains("kruvaziyer") == true);
                
                ViewBag.Summary = new {
                    TotalHotels = hotelProducts.Sum(p => (int)(p.TotalSales ?? 0)),
                    HotelRevenue = hotelProducts.Sum(p => (decimal)(p.TotalRevenue ?? 0)),
                    TotalTours = tourProducts.Sum(p => (int)(p.TotalSales ?? 0)),
                    TourRevenue = tourProducts.Sum(p => (decimal)(p.TotalRevenue ?? 0)),
                    TotalFlights = flightProducts.Sum(p => (int)(p.TotalSales ?? 0)),
                    FlightRevenue = flightProducts.Sum(p => (decimal)(p.TotalRevenue ?? 0)),
                    TotalCruises = cruiseProducts.Sum(p => (int)(p.TotalSales ?? 0)),
                    CruiseRevenue = cruiseProducts.Sum(p => (decimal)(p.TotalRevenue ?? 0))
                };
            }
            catch
            {
                ViewBag.Products = new List<dynamic>();
                ViewBag.Summary = new {
                    TotalHotels = 0,
                    HotelRevenue = 0m,
                    TotalTours = 0,
                    TourRevenue = 0m,
                    TotalFlights = 0,
                    FlightRevenue = 0m,
                    TotalCruises = 0,
                    CruiseRevenue = 0m
                };
            }
            
            return View(report);
        }

        [HttpPost]
        public async Task<IActionResult> ExportCustomerReport(string format = "pdf")
        {
            var response = await _reportsApiService.ExportCustomerReportAsync(format);
            if (response != null && response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsByteArrayAsync();
                var contentType = format.ToLower() == "excel" ? "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" : "application/pdf";
                var fileName = $"CustomerReport.{(format.ToLower() == "excel" ? "xlsx" : "pdf")}";
                
                return File(content, contentType, fileName);
            }
            
            ModelState.AddModelError("", "Rapor dışa aktarılırken hata oluştu.");
            return RedirectToAction(nameof(CustomerReport));
        }

        [HttpPost]
        public async Task<IActionResult> ExportFinancialReport(string format = "pdf")
        {
            var response = await _reportsApiService.ExportFinancialReportAsync(format);
            if (response != null && response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsByteArrayAsync();
                var contentType = format.ToLower() == "excel" ? "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" : "application/pdf";
                var fileName = $"FinancialReport.{(format.ToLower() == "excel" ? "xlsx" : "pdf")}";
                
                return File(content, contentType, fileName);
            }
            
            ModelState.AddModelError("", "Rapor dışa aktarılırken hata oluştu.");
            return RedirectToAction(nameof(FinancialReport));
        }

        [HttpPost]
        public async Task<IActionResult> ExportSalesReport(string format = "pdf")
        {
            var response = await _reportsApiService.ExportSalesReportAsync(format);
            if (response != null && response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsByteArrayAsync();
                var contentType = format.ToLower() == "excel" ? "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" : "application/pdf";
                var fileName = $"SalesReport.{(format.ToLower() == "excel" ? "xlsx" : "pdf")}";
                
                return File(content, contentType, fileName);
            }
            
            ModelState.AddModelError("", "Rapor dışa aktarılırken hata oluştu.");
            return RedirectToAction(nameof(SalesReport));
        }

        [HttpPost]
        public async Task<IActionResult> ExportProductReport(string format = "pdf")
        {
            var response = await _reportsApiService.ExportProductReportAsync(format);
            if (response != null && response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsByteArrayAsync();
                var contentType = format.ToLower() == "excel" ? "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" : "application/pdf";
                var fileName = $"ProductReport.{(format.ToLower() == "excel" ? "xlsx" : "pdf")}";
                
                return File(content, contentType, fileName);
            }
            
            ModelState.AddModelError("", "Rapor dışa aktarılırken hata oluştu.");
            return RedirectToAction(nameof(ProductReport));
        }

        private async Task LoadLookupData()
        {
            var currencies = await _lookupApiService.GetCurrenciesAsync() ?? new List<dynamic>();
            ViewBag.Currencies = currencies;

            var productTypes = await _lookupApiService.GetProductTypesAsync() ?? new List<dynamic>();
            ViewBag.ProductTypes = productTypes;
        }
    }
}