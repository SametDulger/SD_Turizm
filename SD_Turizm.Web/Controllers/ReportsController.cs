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

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> CustomerReport()
        {
            await LoadLookupData();
            var report = await _reportsApiService.GetCustomerReportDataAsync() ?? new { };
            return View(report);
        }

        public async Task<IActionResult> FinancialReport()
        {
            await LoadLookupData();
            var report = await _reportsApiService.GetFinancialReportDataAsync() ?? new { };
            return View(report);
        }

        public async Task<IActionResult> SalesReport()
        {
            await LoadLookupData();
            var report = await _reportsApiService.GetSalesReportDataAsync() ?? new { };
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

        private async Task LoadLookupData()
        {
            var currencies = await _lookupApiService.GetCurrenciesAsync() ?? new List<dynamic>();
            ViewBag.Currencies = currencies;

            var productTypes = await _lookupApiService.GetProductTypesAsync() ?? new List<dynamic>();
            ViewBag.ProductTypes = productTypes;
        }
    }
}