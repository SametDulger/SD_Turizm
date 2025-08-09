using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SD_Turizm.Application.Services;
using SD_Turizm.Core.Entities;

namespace SD_Turizm.API.Controllers.V1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Authorize]
    public class HotelsController : ControllerBase
    {
        private readonly IHotelService _hotelService;

        public HotelsController(IHotelService hotelService)
        {
            _hotelService = hotelService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Hotel>>> GetAll()
        {
            var hotels = await _hotelService.GetAllHotelsAsync();
            return Ok(hotels);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Hotel>> GetById(int id)
        {
            var hotel = await _hotelService.GetByIdAsync(id);
            if (hotel == null)
                return NotFound();
            return Ok(hotel);
        }

        [HttpGet("code/{code}")]
        public async Task<ActionResult<Hotel>> GetByCode(string code)
        {
            var hotel = await _hotelService.GetByCodeAsync(code);
            if (hotel == null)
                return NotFound();
            return Ok(hotel);
        }

        [HttpPost]
        public async Task<ActionResult<Hotel>> Create(Hotel hotel)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (hotel == null)
                return BadRequest("Hotel data is required");

            if (string.IsNullOrWhiteSpace(hotel.Name))
                return BadRequest("Hotel name is required");

            if (string.IsNullOrWhiteSpace(hotel.Code))
                return BadRequest("Hotel code is required");

            var createdHotel = await _hotelService.CreateAsync(hotel);
            return CreatedAtAction(nameof(GetById), new { id = createdHotel.Id }, createdHotel);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Hotel hotel)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (hotel == null)
                return BadRequest("Hotel data is required");

            if (id != hotel.Id)
                return BadRequest("ID mismatch");

            if (string.IsNullOrWhiteSpace(hotel.Name))
                return BadRequest("Hotel name is required");

            if (string.IsNullOrWhiteSpace(hotel.Code))
                return BadRequest("Hotel code is required");

            if (!await _hotelService.ExistsAsync(id))
                return NotFound();

            await _hotelService.UpdateAsync(hotel);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (!await _hotelService.ExistsAsync(id))
                return NotFound();

            await _hotelService.DeleteAsync(id);
            return NoContent();
        }
    }
}
