using Microsoft.AspNetCore.Mvc;
using SD_Turizm.Application.Services;
using SD_Turizm.Core.Entities.Prices;
using SD_Turizm.Core.DTOs;

namespace SD_Turizm.API.Controllers.V2
{
    [ApiController]
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class HotelPriceController : ControllerBase
    {
        private readonly IHotelPriceService _service;
        private readonly ILoggingService _loggingService;

        public HotelPriceController(IHotelPriceService service, ILoggingService loggingService)
        {
            _service = service;
            _loggingService = loggingService;
        }

        [HttpGet]
        public async Task<ActionResult<PagedResult<HotelPrice>>> GetAll(
            [FromQuery] int page = 1, [FromQuery] int pageSize = 20,
            [FromQuery] string? sortBy = "price", [FromQuery] string? sortOrder = "asc",
            [FromQuery] int? hotelId = null, [FromQuery] decimal? minPrice = null,
            [FromQuery] decimal? maxPrice = null, [FromQuery] DateTime? startDate = null,
            [FromQuery] DateTime? endDate = null)
        {
            try
            {
                _loggingService.LogInformation("Getting hotel prices with pagination", new { page, pageSize, hotelId, minPrice, maxPrice, startDate, endDate });
                
                var pagination = new PaginationDto { Page = page, PageSize = pageSize };
                var result = await _service.GetHotelPricesWithPaginationAsync(pagination, hotelId, minPrice, maxPrice, startDate, endDate);
                
                return Ok(result);
            }
            catch (Exception ex)
            {
                _loggingService.LogError("Error getting hotel prices", ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<HotelPrice>> GetById(int id)
        {
            try
            {
                _loggingService.LogInformation("Getting hotel price by ID", new { id });
                
                var entity = await _service.GetByIdAsync(id);
                if (entity == null)
                {
                    _loggingService.LogWarning("Hotel price not found", new { id });
                    return NotFound();
                }
                
                return Ok(entity);
            }
            catch (Exception ex)
            {
                _loggingService.LogError($"Error getting hotel price by ID: {id}", ex, new { id });
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("hotel/{hotelId}")]
        public async Task<ActionResult<IEnumerable<HotelPrice>>> GetByHotelId(int hotelId)
        {
            try
            {
                _loggingService.LogInformation("Getting hotel prices by hotel ID", new { hotelId });
                
                var prices = await _service.GetPricesByHotelIdAsync(hotelId);
                return Ok(prices);
            }
            catch (Exception ex)
            {
                _loggingService.LogError($"Error getting hotel prices by hotel ID: {hotelId}", ex, new { hotelId });
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("stats")]
        public async Task<ActionResult<object>> GetPriceStats()
        {
            try
            {
                _loggingService.LogInformation("Getting hotel price statistics");
                
                var stats = await _service.GetPriceStatisticsAsync();
                return Ok(stats);
            }
            catch (Exception ex)
            {
                _loggingService.LogError("Error getting hotel price statistics", ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<ActionResult<HotelPrice>> Create(HotelPrice entity)
        {
            try
            {
                _loggingService.LogInformation("Creating new hotel price", new { entity.HotelId, entity.AdultPrice });
                
                var createdEntity = await _service.CreateAsync(entity);
                return CreatedAtAction(nameof(GetById), new { id = createdEntity.Id }, createdEntity);
            }
            catch (Exception ex)
            {
                _loggingService.LogError("Error creating hotel price", ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, HotelPrice entity)
        {
            try
            {
                _loggingService.LogInformation("Updating hotel price", new { id, entity.AdultPrice });
                
                if (id != entity.Id)
                    return BadRequest();

                if (!await _service.ExistsAsync(id))
                {
                    _loggingService.LogWarning("Hotel price not found for update", new { id });
                    return NotFound();
                }

                await _service.UpdateAsync(entity);
                return NoContent();
            }
            catch (Exception ex)
            {
                _loggingService.LogError($"Error updating hotel price: {id}", ex, new { id });
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                _loggingService.LogInformation("Deleting hotel price", new { id });
                
                if (!await _service.ExistsAsync(id))
                {
                    _loggingService.LogWarning("Hotel price not found for deletion", new { id });
                    return NotFound();
                }

                await _service.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _loggingService.LogError($"Error deleting hotel price: {id}", ex, new { id });
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
