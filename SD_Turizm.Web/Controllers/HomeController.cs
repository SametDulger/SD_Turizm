using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SD_Turizm.Web.Models;
using System.Text.Json;

namespace SD_Turizm.Web.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly HttpClient _httpClient;
    private readonly string _apiBaseUrl;

    public HomeController(ILogger<HomeController> logger, HttpClient httpClient, IConfiguration configuration)
    {
        _logger = logger;
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
                var jsonElement = JsonSerializer.Deserialize<JsonElement>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                
                ViewBag.Statistics = new
                {
                    TotalSales = jsonElement.TryGetProperty("totalSales", out var totalSales) ? totalSales.GetDecimal() : 0,
                    ActiveTours = jsonElement.TryGetProperty("activeTours", out var activeTours) ? activeTours.GetInt32() : 0,
                    TotalHotels = jsonElement.TryGetProperty("totalHotels", out var totalHotels) ? totalHotels.GetInt32() : 0,
                    TotalCustomers = jsonElement.TryGetProperty("totalCustomers", out var totalCustomers) ? totalCustomers.GetInt32() : 0
                };
            }
            else
            {
                ViewBag.Statistics = new
                {
                    TotalSales = 0,
                    ActiveTours = 0,
                    TotalHotels = 0,
                    TotalCustomers = 0
                };
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Dashboard istatistikleri alınırken hata oluştu");
            ViewBag.Statistics = new
            {
                TotalSales = 0,
                ActiveTours = 0,
                TotalHotels = 0,
                TotalCustomers = 0
            };
        }

        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
