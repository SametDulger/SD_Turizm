using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SD_Turizm.Web.Models.DTOs;
using System.Text;
using System.Text.Json;

namespace SD_Turizm.Web.Controllers
{
    public class GuidePriceController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiBaseUrl;

        public GuidePriceController(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _apiBaseUrl = configuration["ApiBaseUrl"] ?? "http://localhost:7000/api/";
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_apiBaseUrl}GuidePrice");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var entities = JsonSerializer.Deserialize<List<GuidePriceDto>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    return View(entities);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Hata oluştu: {ex.Message}");
            }
            return View(new List<GuidePriceDto>());
        }

        public async Task<IActionResult> Create()
        {
            try
            {
                var guidesResponse = await _httpClient.GetAsync($"{_apiBaseUrl}Guide");
                if (guidesResponse.IsSuccessStatusCode)
                {
                    var guidesContent = await guidesResponse.Content.ReadAsStringAsync();
                    var guides = JsonSerializer.Deserialize<List<GuideDto>>(guidesContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    ViewBag.GuideId = guides?.Select(g => new SelectListItem { Value = g.Id.ToString(), Text = $"{g.Code} - {g.Name}" }).ToList();
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Rehber listesi yüklenirken hata oluştu: {ex.Message}");
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(GuidePriceDto entity)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var json = JsonSerializer.Serialize(entity);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    var response = await _httpClient.PostAsync($"{_apiBaseUrl}GuidePrice", content);
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
                var response = await _httpClient.GetAsync($"{_apiBaseUrl}GuidePrice/{id}");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var entity = JsonSerializer.Deserialize<GuidePriceDto>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
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
                var response = await _httpClient.GetAsync($"{_apiBaseUrl}GuidePrice/{id}");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var entity = JsonSerializer.Deserialize<GuidePriceDto>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    
                    var guidesResponse = await _httpClient.GetAsync($"{_apiBaseUrl}Guide");
                    if (guidesResponse.IsSuccessStatusCode)
                    {
                        var guidesContent = await guidesResponse.Content.ReadAsStringAsync();
                        var guides = JsonSerializer.Deserialize<List<GuideDto>>(guidesContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                        ViewBag.GuideId = guides?.Select(g => new SelectListItem { Value = g.Id.ToString(), Text = $"{g.Code} - {g.Name}" }).ToList();
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
        public async Task<IActionResult> Edit(int id, GuidePriceDto entity)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var json = JsonSerializer.Serialize(entity);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    var response = await _httpClient.PutAsync($"{_apiBaseUrl}GuidePrice/{id}", content);
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
                var response = await _httpClient.GetAsync($"{_apiBaseUrl}GuidePrice/{id}");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var entity = JsonSerializer.Deserialize<GuidePriceDto>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
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
                var response = await _httpClient.DeleteAsync($"{_apiBaseUrl}GuidePrice/{id}");
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
