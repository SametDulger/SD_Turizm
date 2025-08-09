using Microsoft.AspNetCore.Mvc;
using SD_Turizm.Application.Services;
using SD_Turizm.Core.Entities.Prices;
using SD_Turizm.Core.DTOs;

namespace SD_Turizm.API.Controllers.V2
{
    [ApiController]
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class CruisePriceController : ControllerBase
    {
        private readonly ICruisePriceService _service;
        private readonly ILoggingService _loggingService;

        public CruisePriceController(ICruisePriceService service, ILoggingService loggingService)
        {
            _service = service;
            _loggingService = loggingService;
        }

        [HttpGet]
        public async Task<ActionResult<PagedResult<CruisePrice>>> GetAll(
            [FromQuery] int page = 1, [FromQuery] int pageSize = 20,
            [FromQuery] string? sortBy = "price", [FromQuery] string? sortOrder = "asc",
            [FromQuery] int? cruiseId = null, [FromQuery] decimal? minPrice = null,
            [FromQuery] decimal? maxPrice = null, [FromQuery] DateTime? startDate = null,
            [FromQuery] DateTime? endDate = null)
        {
            try
            {
                _loggingService.LogInformation("Getting cruise prices with pagination", new { page, pageSize, cruiseId, minPrice, maxPrice, startDate, endDate });
                
                var pagination = new PaginationDto { Page = page, PageSize = pageSize };
                var result = await _service.GetCruisePricesWithPaginationAsync(pagination, cruiseId, minPrice, maxPrice, startDate, endDate);
                
                return Ok(result);
            }
            catch (Exception ex)
            {
                _loggingService.LogError("Error getting cruise prices", ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CruisePrice>> GetById(int id)
        {
            try
            {
                _loggingService.LogInformation("Getting cruise price by ID", new { id });
                
                var entity = await _service.GetByIdAsync(id);
                if (entity == null)
                {
                    _loggingService.LogWarning("Cruise price not found", new { id });
                    return NotFound();
                }
                
                return Ok(entity);
            }
            catch (Exception ex)
            {
                _loggingService.LogError($"Error getting cruise price by ID: {id}", ex, new { id });
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("cruise/{cruiseId}")]
        public async Task<ActionResult<IEnumerable<CruisePrice>>> GetByCruiseId(int cruiseId)
        {
            try
            {
                _loggingService.LogInformation("Getting cruise prices by cruise ID", new { cruiseId });
                
                var prices = await _service.GetPricesByCruiseIdAsync(cruiseId);
                return Ok(prices);
            }
            catch (Exception ex)
            {
                _loggingService.LogError($"Error getting cruise prices by cruise ID: {cruiseId}", ex, new { cruiseId });
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("stats")]
        public async Task<ActionResult<object>> GetPriceStats()
        {
            try
            {
                _loggingService.LogInformation("Getting cruise price statistics");
                
                var stats = await _service.GetPriceStatisticsAsync();
                return Ok(stats);
            }
            catch (Exception ex)
            {
                _loggingService.LogError("Error getting cruise price statistics", ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<ActionResult<CruisePrice>> Create(CruisePrice entity)
        {
            try
            {
                _loggingService.LogInformation("Creating new cruise price", new { entity.CruiseId, entity.AdultPrice });
                
                var createdEntity = await _service.CreateAsync(entity);
                return CreatedAtAction(nameof(GetById), new { id = createdEntity.Id }, createdEntity);
            }
            catch (Exception ex)
            {
                _loggingService.LogError("Error creating cruise price", ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, CruisePrice entity)
        {
            try
            {
                _loggingService.LogInformation("Updating cruise price", new { id, entity.AdultPrice });
                
                if (id != entity.Id)
                    return BadRequest();

                if (!await _service.ExistsAsync(id))
                {
                    _loggingService.LogWarning("Cruise price not found for update", new { id });
                    return NotFound();
                }

                await _service.UpdateAsync(entity);
                return NoContent();
            }
            catch (Exception ex)
            {
                _loggingService.LogError($"Error updating cruise price: {id}", ex, new { id });
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                _loggingService.LogInformation("Deleting cruise price", new { id });
                
                if (!await _service.ExistsAsync(id))
                {
                    _loggingService.LogWarning("Cruise price not found for deletion", new { id });
                    return NotFound();
                }

                await _service.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _loggingService.LogError($"Error deleting cruise price: {id}", ex, new { id });
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
