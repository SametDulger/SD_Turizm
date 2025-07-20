using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SD_Turizm.Web.Models.DTOs;
using System.Text;
using System.Text.Json;

namespace SD_Turizm.Web.Controllers
{
    public class HotelPriceController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiBaseUrl;

        public HotelPriceController(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _apiBaseUrl = configuration["ApiBaseUrl"] ?? "http://localhost:7000/api/";
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_apiBaseUrl}HotelPrice");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var entities = JsonSerializer.Deserialize<List<HotelPriceDto>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    return View(entities);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Hata oluştu: {ex.Message}");
            }
            return View(new List<HotelPriceDto>());
        }

        public async Task<IActionResult> Create()
        {
            try
            {
                var hotelsResponse = await _httpClient.GetAsync($"{_apiBaseUrl}Hotels");
                if (hotelsResponse.IsSuccessStatusCode)
                {
                    var hotelsContent = await hotelsResponse.Content.ReadAsStringAsync();
                    var hotels = JsonSerializer.Deserialize<List<HotelDto>>(hotelsContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    ViewBag.HotelId = hotels?.Select(h => new SelectListItem { Value = h.Id.ToString(), Text = $"{h.Code} - {h.Name}" }).ToList();
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Otel listesi yüklenirken hata oluştu: {ex.Message}");
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(HotelPriceDto entity)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var json = JsonSerializer.Serialize(entity);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    var response = await _httpClient.PostAsync($"{_apiBaseUrl}HotelPrice", content);
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
                var response = await _httpClient.GetAsync($"{_apiBaseUrl}HotelPrice/{id}");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var entity = JsonSerializer.Deserialize<HotelPriceDto>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
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
                var response = await _httpClient.GetAsync($"{_apiBaseUrl}HotelPrice/{id}");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var entity = JsonSerializer.Deserialize<HotelPriceDto>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    
                    var hotelsResponse = await _httpClient.GetAsync($"{_apiBaseUrl}Hotels");
                    if (hotelsResponse.IsSuccessStatusCode)
                    {
                        var hotelsContent = await hotelsResponse.Content.ReadAsStringAsync();
                        var hotels = JsonSerializer.Deserialize<List<HotelDto>>(hotelsContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                        ViewBag.HotelId = hotels?.Select(h => new SelectListItem { Value = h.Id.ToString(), Text = $"{h.Code} - {h.Name}" }).ToList();
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
        public async Task<IActionResult> Edit(int id, HotelPriceDto entity)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var json = JsonSerializer.Serialize(entity);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    var response = await _httpClient.PutAsync($"{_apiBaseUrl}HotelPrice/{id}", content);
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
                var response = await _httpClient.GetAsync($"{_apiBaseUrl}HotelPrice/{id}");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var entity = JsonSerializer.Deserialize<HotelPriceDto>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
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
                var response = await _httpClient.DeleteAsync($"{_apiBaseUrl}HotelPrice/{id}");
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
