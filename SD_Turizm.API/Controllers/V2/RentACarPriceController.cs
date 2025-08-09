using Microsoft.AspNetCore.Mvc;
using SD_Turizm.Application.Services;
using SD_Turizm.Core.Entities.Prices;
using SD_Turizm.Core.DTOs;

namespace SD_Turizm.API.Controllers.V2
{
    [ApiController]
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class RentACarPriceController : ControllerBase
    {
        private readonly IRentACarPriceService _service;
        private readonly ILoggingService _loggingService;

        public RentACarPriceController(IRentACarPriceService service, ILoggingService loggingService)
        {
            _service = service;
            _loggingService = loggingService;
        }

        [HttpGet]
        public async Task<ActionResult<PagedResult<RentACarPrice>>> GetAll(
            [FromQuery] int page = 1, [FromQuery] int pageSize = 20,
            [FromQuery] string? sortBy = "price", [FromQuery] string? sortOrder = "asc",
            [FromQuery] int? rentACarId = null, [FromQuery] decimal? minPrice = null,
            [FromQuery] decimal? maxPrice = null, [FromQuery] DateTime? startDate = null,
            [FromQuery] DateTime? endDate = null)
        {
            try
            {
                _loggingService.LogInformation("Getting rent a car prices with pagination", new { page, pageSize, rentACarId, minPrice, maxPrice, startDate, endDate });
                
                var pagination = new PaginationDto { Page = page, PageSize = pageSize };
                var result = await _service.GetRentACarPricesWithPaginationAsync(pagination, rentACarId, minPrice, maxPrice, startDate, endDate);
                
                return Ok(result);
            }
            catch (Exception ex)
            {
                _loggingService.LogError("Error getting rent a car prices", ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RentACarPrice>> GetById(int id)
        {
            try
            {
                _loggingService.LogInformation("Getting rent a car price by ID", new { id });
                
                var entity = await _service.GetByIdAsync(id);
                if (entity == null)
                {
                    _loggingService.LogWarning("Rent a car price not found", new { id });
                    return NotFound();
                }
                
                return Ok(entity);
            }
            catch (Exception ex)
            {
                _loggingService.LogError($"Error getting rent a car price by ID: {id}", ex, new { id });
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("rentacar/{rentACarId}")]
        public async Task<ActionResult<IEnumerable<RentACarPrice>>> GetByRentACarId(int rentACarId)
        {
            try
            {
                _loggingService.LogInformation("Getting rent a car prices by rent a car ID", new { rentACarId });
                
                var prices = await _service.GetPricesByRentACarIdAsync(rentACarId);
                return Ok(prices);
            }
            catch (Exception ex)
            {
                _loggingService.LogError($"Error getting rent a car prices by rent a car ID: {rentACarId}", ex, new { rentACarId });
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("stats")]
        public async Task<ActionResult<object>> GetPriceStats()
        {
            try
            {
                _loggingService.LogInformation("Getting rent a car price statistics");
                
                var stats = await _service.GetPriceStatisticsAsync();
                return Ok(stats);
            }
            catch (Exception ex)
            {
                _loggingService.LogError("Error getting rent a car price statistics", ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<ActionResult<RentACarPrice>> Create(RentACarPrice entity)
        {
            try
            {
                _loggingService.LogInformation("Creating new rent a car price", new { entity.RentACarId, entity.AdultPrice });
                
                var createdEntity = await _service.CreateAsync(entity);
                return CreatedAtAction(nameof(GetById), new { id = createdEntity.Id }, createdEntity);
            }
            catch (Exception ex)
            {
                _loggingService.LogError("Error creating rent a car price", ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, RentACarPrice entity)
        {
            try
            {
                _loggingService.LogInformation("Updating rent a car price", new { id, entity.AdultPrice });
                
                if (id != entity.Id)
                    return BadRequest();

                if (!await _service.ExistsAsync(id))
                {
                    _loggingService.LogWarning("Rent a car price not found for update", new { id });
                    return NotFound();
                }

                await _service.UpdateAsync(entity);
                return NoContent();
            }
            catch (Exception ex)
            {
                _loggingService.LogError($"Error updating rent a car price: {id}", ex, new { id });
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                _loggingService.LogInformation("Deleting rent a car price", new { id });
                
                if (!await _service.ExistsAsync(id))
                {
                    _loggingService.LogWarning("Rent a car price not found for deletion", new { id });
                    return NotFound();
                }

                await _service.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _loggingService.LogError($"Error deleting rent a car price: {id}", ex, new { id });
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
