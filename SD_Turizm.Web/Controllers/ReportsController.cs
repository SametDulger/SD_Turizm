using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using SD_Turizm.Web.Models.DTOs;

namespace SD_Turizm.Web.Controllers
{
    public class ReportsController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiBaseUrl;

        public ReportsController(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _apiBaseUrl = configuration["ApiBaseUrl"] ?? "http://localhost:7000/api/";
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_apiBaseUrl}Dashboard/statistics");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var statistics = JsonSerializer.Deserialize<dynamic>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    
                    ViewBag.MonthlySales = statistics?.GetProperty("TotalSales")?.GetDecimal();
                    ViewBag.WeeklySales = statistics?.GetProperty("TotalSales")?.GetDecimal() * 0.25m; // Haftalık tahmin
                    ViewBag.TotalCustomers = statistics?.GetProperty("TotalCustomers")?.GetInt32();
                    ViewBag.ActivePackages = 0; // API'de henüz paket sayısı yok
                }
                else
                {
                    ViewBag.MonthlySales = 0;
                    ViewBag.WeeklySales = 0;
                    ViewBag.TotalCustomers = 0;
                    ViewBag.ActivePackages = 0;
                }
            }
            catch (Exception)
            {
                ViewBag.MonthlySales = 0;
                ViewBag.WeeklySales = 0;
                ViewBag.TotalCustomers = 0;
                ViewBag.ActivePackages = 0;
            }

            return View();
        }

        public async Task<IActionResult> SalesReport(DateTime? startDate = null, DateTime? endDate = null, string? sellerType = null, string? currency = null, string? pnrNumber = null, string? fileCode = null, string? agencyCode = null, string? cariCode = null)
        {
            try
            {
                var start = startDate ?? DateTime.Today.AddDays(-30);
                var end = endDate ?? DateTime.Today;

                var queryParams = new List<string>
                {
                    $"startDate={start:yyyy-MM-dd}",
                    $"endDate={end:yyyy-MM-dd}"
                };

                if (!string.IsNullOrEmpty(sellerType))
                    queryParams.Add($"sellerType={sellerType}");
                if (!string.IsNullOrEmpty(currency))
                    queryParams.Add($"currency={currency}");
                if (!string.IsNullOrEmpty(pnrNumber))
                    queryParams.Add($"pnrNumber={pnrNumber}");
                if (!string.IsNullOrEmpty(fileCode))
                    queryParams.Add($"fileCode={fileCode}");
                if (!string.IsNullOrEmpty(agencyCode))
                    queryParams.Add($"agencyCode={agencyCode}");
                if (!string.IsNullOrEmpty(cariCode))
                    queryParams.Add($"cariCode={cariCode}");

                var queryString = string.Join("&", queryParams);
                var response = await _httpClient.GetAsync($"{_apiBaseUrl}Reports/sales?{queryString}");
                var summaryResponse = await _httpClient.GetAsync($"{_apiBaseUrl}Reports/sales/summary?{queryString}");

                if (response.IsSuccessStatusCode && summaryResponse.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var summaryContent = await summaryResponse.Content.ReadAsStringAsync();
                    
                    var sales = JsonSerializer.Deserialize<List<SalesReportDto>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    var summary = JsonSerializer.Deserialize<SalesSummaryDto>(summaryContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    ViewBag.Sales = sales;
                    ViewBag.Summary = summary;
                    ViewBag.StartDate = start;
                    ViewBag.EndDate = end;
                    ViewBag.Filters = new { sellerType, currency, pnrNumber, fileCode, agencyCode, cariCode };
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Hata oluştu: {ex.Message}");
            }

            return View();
        }

        public async Task<IActionResult> FinancialReport(DateTime? startDate = null, DateTime? endDate = null, string currency = "TRY")
        {
            try
            {
                var start = startDate ?? DateTime.Today.AddDays(-30);
                var end = endDate ?? DateTime.Today;

                var queryString = $"startDate={start:yyyy-MM-dd}&endDate={end:yyyy-MM-dd}&currency={currency}";
                var response = await _httpClient.GetAsync($"{_apiBaseUrl}Reports/financial?{queryString}");
                var summaryResponse = await _httpClient.GetAsync($"{_apiBaseUrl}Reports/financial/summary?{queryString}");

                if (response.IsSuccessStatusCode && summaryResponse.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var summaryContent = await summaryResponse.Content.ReadAsStringAsync();
                    
                    var sales = JsonSerializer.Deserialize<List<FinancialReportDto>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    var summary = JsonSerializer.Deserialize<FinancialSummaryDto>(summaryContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    ViewBag.Sales = sales;
                    ViewBag.Summary = summary;
                    ViewBag.StartDate = start;
                    ViewBag.EndDate = end;
                    ViewBag.Currency = currency;
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Hata oluştu: {ex.Message}");
            }

            return View();
        }

        public async Task<IActionResult> CustomerReport(DateTime? startDate = null, DateTime? endDate = null, string? cariCode = null)
        {
            try
            {
                var start = startDate ?? DateTime.Today.AddDays(-30);
                var end = endDate ?? DateTime.Today;

                var queryParams = new List<string>
                {
                    $"startDate={start:yyyy-MM-dd}",
                    $"endDate={end:yyyy-MM-dd}"
                };

                if (!string.IsNullOrEmpty(cariCode))
                    queryParams.Add($"cariCode={cariCode}");

                var queryString = string.Join("&", queryParams);
                var response = await _httpClient.GetAsync($"{_apiBaseUrl}Reports/customers?{queryString}");
                var summaryResponse = await _httpClient.GetAsync($"{_apiBaseUrl}Reports/customers/summary?{queryString}");

                if (response.IsSuccessStatusCode && summaryResponse.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var summaryContent = await summaryResponse.Content.ReadAsStringAsync();
                    
                    var sales = JsonSerializer.Deserialize<List<CustomerReportDto>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    var summary = JsonSerializer.Deserialize<CustomerSummaryDto>(summaryContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    ViewBag.Sales = sales;
                    ViewBag.Summary = summary;
                    ViewBag.StartDate = start;
                    ViewBag.EndDate = end;
                    ViewBag.CariCode = cariCode;
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Hata oluştu: {ex.Message}");
            }

            return View();
        }

        public async Task<IActionResult> ProductReport(DateTime? startDate = null, DateTime? endDate = null, string? productType = null)
        {
            try
            {
                var start = startDate ?? DateTime.Today.AddDays(-30);
                var end = endDate ?? DateTime.Today;

                var queryParams = new List<string>
                {
                    $"startDate={start:yyyy-MM-dd}",
                    $"endDate={end:yyyy-MM-dd}"
                };

                if (!string.IsNullOrEmpty(productType))
                    queryParams.Add($"productType={productType}");

                var queryString = string.Join("&", queryParams);
                var response = await _httpClient.GetAsync($"{_apiBaseUrl}Reports/products?{queryString}");
                var summaryResponse = await _httpClient.GetAsync($"{_apiBaseUrl}Reports/products/summary?{queryString}");

                if (response.IsSuccessStatusCode && summaryResponse.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var summaryContent = await summaryResponse.Content.ReadAsStringAsync();
                    
                    var products = JsonSerializer.Deserialize<List<ProductReportDto>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    var summary = JsonSerializer.Deserialize<ProductSummaryDto>(summaryContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    ViewBag.Products = products;
                    ViewBag.Summary = summary;
                    ViewBag.StartDate = start;
                    ViewBag.EndDate = end;
                    ViewBag.ProductType = productType;
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Hata oluştu: {ex.Message}");
            }

            return View();
        }
    }
} 