using Microsoft.AspNetCore.Mvc;
using SD_Turizm.Application.Services;
using SD_Turizm.Core.Entities.Prices;
using SD_Turizm.Core.DTOs;

namespace SD_Turizm.API.Controllers.V2
{
    [ApiController]
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class AirlinePriceController : ControllerBase
    {
        private readonly IAirlinePriceService _service;
        private readonly ILoggingService _loggingService;

        public AirlinePriceController(IAirlinePriceService service, ILoggingService loggingService)
        {
            _service = service;
            _loggingService = loggingService;
        }

        [HttpGet]
        public async Task<ActionResult<PagedResult<AirlinePrice>>> GetAll(
            [FromQuery] int page = 1, [FromQuery] int pageSize = 20,
            [FromQuery] string? sortBy = "price", [FromQuery] string? sortOrder = "asc",
            [FromQuery] int? airlineId = null, [FromQuery] decimal? minPrice = null,
            [FromQuery] decimal? maxPrice = null, [FromQuery] DateTime? startDate = null,
            [FromQuery] DateTime? endDate = null)
        {
            try
            {
                _loggingService.LogInformation("Getting airline prices with pagination", new { page, pageSize, airlineId, minPrice, maxPrice, startDate, endDate });
                
                var pagination = new PaginationDto { Page = page, PageSize = pageSize };
                var result = await _service.GetAirlinePricesWithPaginationAsync(pagination, airlineId, minPrice, maxPrice, startDate, endDate);
                
                return Ok(result);
            }
            catch (Exception ex)
            {
                _loggingService.LogError("Error getting airline prices", ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AirlinePrice>> GetById(int id)
        {
            try
            {
                _loggingService.LogInformation("Getting airline price by ID", new { id });
                
                var entity = await _service.GetByIdAsync(id);
                if (entity == null)
                {
                    _loggingService.LogWarning("Airline price not found", new { id });
                    return NotFound();
                }
                
                return Ok(entity);
            }
            catch (Exception ex)
            {
                _loggingService.LogError($"Error getting airline price by ID: {id}", ex, new { id });
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("airline/{airlineId}")]
        public async Task<ActionResult<IEnumerable<AirlinePrice>>> GetByAirlineId(int airlineId)
        {
            try
            {
                _loggingService.LogInformation("Getting airline prices by airline ID", new { airlineId });
                
                var prices = await _service.GetPricesByAirlineIdAsync(airlineId);
                return Ok(prices);
            }
            catch (Exception ex)
            {
                _loggingService.LogError($"Error getting airline prices by airline ID: {airlineId}", ex, new { airlineId });
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("stats")]
        public async Task<ActionResult<object>> GetPriceStats()
        {
            try
            {
                _loggingService.LogInformation("Getting airline price statistics");
                
                var stats = await _service.GetPriceStatisticsAsync();
                return Ok(stats);
            }
            catch (Exception ex)
            {
                _loggingService.LogError("Error getting airline price statistics", ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<ActionResult<AirlinePrice>> Create(AirlinePrice entity)
        {
            try
            {
                _loggingService.LogInformation("Creating new airline price", new { entity.AirlineId, entity.AdultPrice });
                
                var createdEntity = await _service.CreateAsync(entity);
                return CreatedAtAction(nameof(GetById), new { id = createdEntity.Id }, createdEntity);
            }
            catch (Exception ex)
            {
                _loggingService.LogError("Error creating airline price", ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, AirlinePrice entity)
        {
            try
            {
                _loggingService.LogInformation("Updating airline price", new { id, entity.AdultPrice });
                
                if (id != entity.Id)
                    return BadRequest();

                if (!await _service.ExistsAsync(id))
                {
                    _loggingService.LogWarning("Airline price not found for update", new { id });
                    return NotFound();
                }

                await _service.UpdateAsync(entity);
                return NoContent();
            }
            catch (Exception ex)
            {
                _loggingService.LogError($"Error updating airline price: {id}", ex, new { id });
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                _loggingService.LogInformation("Deleting airline price", new { id });
                
                if (!await _service.ExistsAsync(id))
                {
                    _loggingService.LogWarning("Airline price not found for deletion", new { id });
                    return NotFound();
                }

                await _service.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _loggingService.LogError($"Error deleting airline price: {id}", ex, new { id });
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
