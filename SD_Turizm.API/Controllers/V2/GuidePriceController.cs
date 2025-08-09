using Microsoft.AspNetCore.Mvc;
using SD_Turizm.Application.Services;
using SD_Turizm.Core.Entities.Prices;
using SD_Turizm.Core.DTOs;

namespace SD_Turizm.API.Controllers.V2
{
    [ApiController]
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class GuidePriceController : ControllerBase
    {
        private readonly IGuidePriceService _service;
        private readonly ILoggingService _loggingService;

        public GuidePriceController(IGuidePriceService service, ILoggingService loggingService)
        {
            _service = service;
            _loggingService = loggingService;
        }

        [HttpGet]
        public async Task<ActionResult<PagedResult<GuidePrice>>> GetAll(
            [FromQuery] int page = 1, [FromQuery] int pageSize = 20,
            [FromQuery] string? sortBy = "price", [FromQuery] string? sortOrder = "asc",
            [FromQuery] int? guideId = null, [FromQuery] decimal? minPrice = null,
            [FromQuery] decimal? maxPrice = null, [FromQuery] DateTime? startDate = null,
            [FromQuery] DateTime? endDate = null)
        {
            try
            {
                _loggingService.LogInformation("Getting guide prices with pagination", new { page, pageSize, guideId, minPrice, maxPrice, startDate, endDate });
                
                var pagination = new PaginationDto { Page = page, PageSize = pageSize };
                var result = await _service.GetGuidePricesWithPaginationAsync(pagination, guideId, minPrice, maxPrice, startDate, endDate);
                
                return Ok(result);
            }
            catch (Exception ex)
            {
                _loggingService.LogError("Error getting guide prices", ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GuidePrice>> GetById(int id)
        {
            try
            {
                _loggingService.LogInformation("Getting guide price by ID", new { id });
                
                var entity = await _service.GetByIdAsync(id);
                if (entity == null)
                {
                    _loggingService.LogWarning("Guide price not found", new { id });
                    return NotFound();
                }
                
                return Ok(entity);
            }
            catch (Exception ex)
            {
                _loggingService.LogError($"Error getting guide price by ID: {id}", ex, new { id });
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("guide/{guideId}")]
        public async Task<ActionResult<IEnumerable<GuidePrice>>> GetByGuideId(int guideId)
        {
            try
            {
                _loggingService.LogInformation("Getting guide prices by guide ID", new { guideId });
                
                var prices = await _service.GetPricesByGuideIdAsync(guideId);
                return Ok(prices);
            }
            catch (Exception ex)
            {
                _loggingService.LogError($"Error getting guide prices by guide ID: {guideId}", ex, new { guideId });
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("stats")]
        public async Task<ActionResult<object>> GetPriceStats()
        {
            try
            {
                _loggingService.LogInformation("Getting guide price statistics");
                
                var stats = await _service.GetPriceStatisticsAsync();
                return Ok(stats);
            }
            catch (Exception ex)
            {
                _loggingService.LogError("Error getting guide price statistics", ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<ActionResult<GuidePrice>> Create(GuidePrice entity)
        {
            try
            {
                _loggingService.LogInformation("Creating new guide price", new { entity.GuideId, entity.AdultPrice });
                
                var createdEntity = await _service.CreateAsync(entity);
                return CreatedAtAction(nameof(GetById), new { id = createdEntity.Id }, createdEntity);
            }
            catch (Exception ex)
            {
                _loggingService.LogError("Error creating guide price", ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, GuidePrice entity)
        {
            try
            {
                _loggingService.LogInformation("Updating guide price", new { id, entity.AdultPrice });
                
                if (id != entity.Id)
                    return BadRequest();

                if (!await _service.ExistsAsync(id))
                {
                    _loggingService.LogWarning("Guide price not found for update", new { id });
                    return NotFound();
                }

                await _service.UpdateAsync(entity);
                return NoContent();
            }
            catch (Exception ex)
            {
                _loggingService.LogError($"Error updating guide price: {id}", ex, new { id });
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                _loggingService.LogInformation("Deleting guide price", new { id });
                
                if (!await _service.ExistsAsync(id))
                {
                    _loggingService.LogWarning("Guide price not found for deletion", new { id });
                    return NotFound();
                }

                await _service.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _loggingService.LogError($"Error deleting guide price: {id}", ex, new { id });
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
