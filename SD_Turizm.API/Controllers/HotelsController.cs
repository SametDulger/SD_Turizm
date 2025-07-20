using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SD_Turizm.Application.Services;
using SD_Turizm.Core.Entities;

namespace SD_Turizm.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HotelsController : ControllerBase
    {
        private readonly IHotelService _hotelService;

        public HotelsController(IHotelService hotelService)
        {
            _hotelService = hotelService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Hotel>>> GetHotels()
        {
            var hotels = await _hotelService.GetAllHotelsAsync();
            return Ok(hotels);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Hotel>> GetHotel(int id)
        {
            var hotel = await _hotelService.GetHotelByIdAsync(id);
            if (hotel == null)
            {
                return NotFound();
            }
            return Ok(hotel);
        }

        [HttpGet("code/{code}")]
        public async Task<ActionResult<Hotel>> GetHotelByCode(string code)
        {
            var hotel = await _hotelService.GetHotelByCodeAsync(code);
            if (hotel == null)
            {
                return NotFound();
            }
            return Ok(hotel);
        }

        [HttpPost]
        public async Task<ActionResult<Hotel>> CreateHotel(Hotel hotel)
        {
            if (await _hotelService.HotelCodeExistsAsync(hotel.Code))
            {
                return BadRequest("Hotel code already exists");
            }

            var createdHotel = await _hotelService.CreateHotelAsync(hotel);
            return CreatedAtAction(nameof(GetHotel), new { id = createdHotel.Id }, createdHotel);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateHotel(int id, Hotel hotel)
        {
            if (id != hotel.Id)
            {
                return BadRequest();
            }

            if (!await _hotelService.HotelExistsAsync(id))
            {
                return NotFound();
            }

            await _hotelService.UpdateHotelAsync(hotel);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHotel(int id)
        {
            if (!await _hotelService.HotelExistsAsync(id))
            {
                return NotFound();
            }

            await _hotelService.DeleteHotelAsync(id);
            return NoContent();
        }
    }
} 