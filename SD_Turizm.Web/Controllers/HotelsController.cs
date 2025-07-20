using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SD_Turizm.Web.Models.DTOs;

namespace SD_Turizm.Web.Controllers
{
    public class HotelsController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiBaseUrl;

        public HotelsController(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _apiBaseUrl = configuration["ApiBaseUrl"] ?? "http://localhost:7000/api/";
        }

        public async Task<IActionResult> Index()
        {
            var hotels = await _httpClient.GetFromJsonAsync<List<HotelDto>>($"{_apiBaseUrl}Hotels");
            return View(hotels);
        }

        public async Task<IActionResult> Details(int id)
        {
            var hotel = await _httpClient.GetFromJsonAsync<HotelDto>($"{_apiBaseUrl}Hotels/{id}");
            if (hotel == null)
            {
                return NotFound();
            }
            return View(hotel);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(HotelDto hotel)
        {
            if (ModelState.IsValid)
            {
                var response = await _httpClient.PostAsJsonAsync($"{_apiBaseUrl}Hotels", hotel);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("", "Error creating hotel");
            }
            return View(hotel);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var hotel = await _httpClient.GetFromJsonAsync<HotelDto>($"{_apiBaseUrl}Hotels/{id}");
            if (hotel == null)
            {
                return NotFound();
            }
            return View(hotel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, HotelDto hotel)
        {
            if (id != hotel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var response = await _httpClient.PutAsJsonAsync($"{_apiBaseUrl}Hotels/{id}", hotel);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("", "Error updating hotel");
            }
            return View(hotel);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var hotel = await _httpClient.GetFromJsonAsync<HotelDto>($"{_apiBaseUrl}Hotels/{id}");
            if (hotel == null)
            {
                return NotFound();
            }
            return View(hotel);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var response = await _httpClient.DeleteAsync($"{_apiBaseUrl}Hotels/{id}");
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Index));
        }
    }
} 
