using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SD_Turizm.Web.Models.DTOs;
using System.Text;
using System.Text.Json;

namespace SD_Turizm.Web.Controllers
{
    public class SaleItemController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiBaseUrl;

        public SaleItemController(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _apiBaseUrl = configuration["ApiBaseUrl"] ?? "http://localhost:7000/api/";
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_apiBaseUrl}SaleItem");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var entities = JsonSerializer.Deserialize<List<SaleItemDto>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    return View(entities);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Hata oluştu: {ex.Message}");
            }
            return View(new List<SaleItemDto>());
        }

        public async Task<IActionResult> Create()
        {
            try
            {
                var salesResponse = await _httpClient.GetAsync($"{_apiBaseUrl}Sales");
                if (salesResponse.IsSuccessStatusCode)
                {
                    var salesContent = await salesResponse.Content.ReadAsStringAsync();
                    var sales = JsonSerializer.Deserialize<List<SaleDto>>(salesContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    ViewBag.Sales = sales?.Select(s => new SelectListItem { Value = s.Id.ToString(), Text = $"{s.SaleNumber} - {s.CustomerName}" }).ToList();
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Satış listesi yüklenirken hata oluştu: {ex.Message}");
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SaleItemDto entity)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var json = JsonSerializer.Serialize(entity);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    var response = await _httpClient.PostAsync($"{_apiBaseUrl}SaleItem", content);
                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Hata oluştu: {ex.Message}");
                }
            }
            return View(entity);
        }

        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_apiBaseUrl}SaleItem/{id}");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var entity = JsonSerializer.Deserialize<SaleItemDto>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    return View(entity);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Hata oluştu: {ex.Message}");
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_apiBaseUrl}SaleItem/{id}");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var entity = JsonSerializer.Deserialize<SaleItemDto>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    
                    var salesResponse = await _httpClient.GetAsync($"{_apiBaseUrl}Sales");
                    if (salesResponse.IsSuccessStatusCode)
                    {
                        var salesContent = await salesResponse.Content.ReadAsStringAsync();
                        var sales = JsonSerializer.Deserialize<List<SaleDto>>(salesContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                        ViewBag.Sales = sales?.Select(s => new SelectListItem { Value = s.Id.ToString(), Text = $"{s.SaleNumber} - {s.CustomerName}" }).ToList();
                    }
                    
                    return View(entity);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Hata oluştu: {ex.Message}");
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, SaleItemDto entity)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var json = JsonSerializer.Serialize(entity);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    var response = await _httpClient.PutAsync($"{_apiBaseUrl}SaleItem/{id}", content);
                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Hata oluştu: {ex.Message}");
                }
            }
            return View(entity);
        }

        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_apiBaseUrl}SaleItem/{id}");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var entity = JsonSerializer.Deserialize<SaleItemDto>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    return View(entity);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Hata oluştu: {ex.Message}");
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"{_apiBaseUrl}SaleItem/{id}");
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Hata oluştu: {ex.Message}");
            }
            return RedirectToAction(nameof(Index));
        }
    }
} 
