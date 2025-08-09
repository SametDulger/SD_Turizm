using Microsoft.AspNetCore.Mvc;
using SD_Turizm.Application.Services;
using SD_Turizm.Core.DTOs;
using SD_Turizm.Core.Entities;

namespace SD_Turizm.API.Controllers.V2
{
    [ApiController]
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class HotelsController : ControllerBase
    {
        private readonly IHotelService _hotelService;
        private readonly ILoggingService _loggingService;

        public HotelsController(IHotelService hotelService, ILoggingService loggingService)
        {
            _hotelService = hotelService;
            _loggingService = loggingService;
        }

        [HttpGet]
        public async Task<ActionResult<PagedResult<Hotel>>> GetAll(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20,
            [FromQuery] string? sortBy = "name",
            [FromQuery] string? sortOrder = "asc",
            [FromQuery] string? searchTerm = null,
            [FromQuery] int? stars = null,
            [FromQuery] string? location = null,
            [FromQuery] string? amenities = null,
            [FromQuery] bool? isActive = null)
        {
            try
            {
                var paginationDto = new PaginationDto
                {
                    Page = page,
                    PageSize = pageSize
                };

                var result = await _hotelService.GetHotelsWithPaginationAsync(
                    paginationDto, searchTerm, location, stars, amenities);

                _loggingService.LogInformation("Hotels retrieved with pagination", new { page, pageSize, filters = new { searchTerm, stars, location } });

                return Ok(result);
            }
                            catch (Exception ex)
                {
                    _loggingService.LogError("Error retrieving hotels with pagination", ex);
                    return StatusCode(500, "Internal server error");
                }
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

        [HttpGet("search")]
        public async Task<ActionResult<PagedResult<Hotel>>> SearchHotels(
            [FromQuery] string location,
            [FromQuery] int? stars = null,
            [FromQuery] string? amenities = null,
            [FromQuery] decimal? minPrice = null,
            [FromQuery] decimal? maxPrice = null,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20)
        {
            try
            {
                var paginationDto = new PaginationDto
                {
                    Page = page,
                    PageSize = pageSize
                };

                var result = await _hotelService.SearchHotelsAsync(
                    paginationDto, location, stars, amenities);

                return Ok(result);
            }
                            catch (Exception ex)
                {
                    _loggingService.LogError("Error searching hotels", ex, new { location, stars, amenities });
                    return StatusCode(500, "Internal server error");
                }
        }

        [HttpGet("{id}/availability")]
        public async Task<ActionResult<object>> CheckAvailability(
            int id,
            [FromQuery] DateTime startDate,
            [FromQuery] DateTime endDate,
            [FromQuery] int? guests = null,
            [FromQuery] string? roomType = null)
        {
            try
            {
                var availability = await _hotelService.CheckAvailabilityAsync(id, startDate, endDate);
                return Ok(availability);
            }
                            catch (Exception ex)
                {
                    _loggingService.LogError("Error checking hotel availability", ex, new { hotelId = id, startDate, endDate });
                    return StatusCode(500, "Internal server error");
                }
        }

        [HttpGet("stats")]
        public async Task<ActionResult<object>> GetHotelStats(
            [FromQuery] string? groupBy = "location",
            [FromQuery] DateTime? startDate = null,
            [FromQuery] DateTime? endDate = null)
        {
            try
            {
                var stats = await _hotelService.GetHotelStatisticsAsync();
                return Ok(stats);
            }
                            catch (Exception ex)
                {
                    _loggingService.LogError("Error retrieving hotel statistics", ex);
                    return StatusCode(500, "Internal server error");
                }
        }

        [HttpPost("bulk-update")]
        public async Task<IActionResult> BulkUpdate([FromBody] List<Hotel> hotels)
        {
            try
            {
                var result = await _hotelService.BulkUpdateAsync(hotels);
                _loggingService.LogInformation("Bulk hotel update completed", new { count = hotels.Count });
                return Ok(new { updatedCount = result });
            }
                            catch (Exception ex)
                {
                    _loggingService.LogError("Error performing bulk hotel update", ex);
                    return StatusCode(500, "Internal server error");
                }
        }

        [HttpPost]
        public async Task<ActionResult<Hotel>> Create(Hotel hotel)
        {
            try
            {
                var createdHotel = await _hotelService.CreateAsync(hotel);
                _loggingService.LogInformation("Hotel created", new { hotelId = createdHotel.Id, code = createdHotel.Code });
                return CreatedAtAction(nameof(GetById), new { id = createdHotel.Id }, createdHotel);
            }
                            catch (Exception ex)
                {
                    _loggingService.LogError("Error creating hotel", ex);
                    return StatusCode(500, "Internal server error");
                }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Hotel hotel)
        {
            if (id != hotel.Id)
                return BadRequest();

            if (!await _hotelService.ExistsAsync(id))
                return NotFound();

            try
            {
                await _hotelService.UpdateAsync(hotel);
                _loggingService.LogInformation("Hotel updated", new { hotelId = id });
                return NoContent();
            }
                            catch (Exception ex)
                {
                    _loggingService.LogError("Error updating hotel", ex, new { hotelId = id });
                    return StatusCode(500, "Internal server error");
                }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (!await _hotelService.ExistsAsync(id))
                return NotFound();

            try
            {
                await _hotelService.DeleteAsync(id);
                _loggingService.LogInformation("Hotel deleted", new { hotelId = id });
                return NoContent();
            }
                            catch (Exception ex)
                {
                    _loggingService.LogError("Error deleting hotel", ex, new { hotelId = id });
                    return StatusCode(500, "Internal server error");
                }
        }
    }
}
