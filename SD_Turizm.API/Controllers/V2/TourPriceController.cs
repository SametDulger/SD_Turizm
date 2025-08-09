using Microsoft.AspNetCore.Mvc;
using SD_Turizm.Application.Services;
using SD_Turizm.Core.Entities.Prices;
using SD_Turizm.Core.DTOs;

namespace SD_Turizm.API.Controllers.V2
{
    [ApiController]
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class TourPriceController : ControllerBase
    {
        private readonly ITourPriceService _service;
        private readonly ILoggingService _loggingService;

        public TourPriceController(ITourPriceService service, ILoggingService loggingService)
        {
            _service = service;
            _loggingService = loggingService;
        }

        [HttpGet]
        public async Task<ActionResult<PagedResult<TourPrice>>> GetAll(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20,
            [FromQuery] string? sortBy = "date",
            [FromQuery] string? sortOrder = "desc",
            [FromQuery] int? tourId = null,
            [FromQuery] decimal? minPrice = null,
            [FromQuery] decimal? maxPrice = null,
            [FromQuery] DateTime? startDate = null,
            [FromQuery] DateTime? endDate = null)
        {
            try
            {
                var paginationDto = new PaginationDto
                {
                    Page = page,
                    PageSize = pageSize
                };

                var result = await _service.GetTourPricesWithPaginationAsync(
                    paginationDto, tourId, minPrice, maxPrice, startDate, endDate);

                _loggingService.LogInformation("Tour prices retrieved with pagination", new { page, pageSize, tourId });

                return Ok(result);
            }
            catch (Exception ex)
            {
                _loggingService.LogError("Error retrieving tour prices with pagination", ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TourPrice>> GetById(int id)
        {
            try
            {
                var entity = await _service.GetByIdAsync(id);
                if (entity == null)
                    return NotFound();
                
                _loggingService.LogInformation("Tour price retrieved", new { priceId = id });
                return Ok(entity);
            }
            catch (Exception ex)
            {
                _loggingService.LogError("Error retrieving tour price", ex, new { priceId = id });
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("tour/{tourId}")]
        public async Task<ActionResult<IEnumerable<TourPrice>>> GetByTourId(int tourId)
        {
            try
            {
                var prices = await _service.GetPricesByTourIdAsync(tourId);
                _loggingService.LogInformation("Tour prices retrieved by tour ID", new { tourId });
                return Ok(prices);
            }
            catch (Exception ex)
            {
                _loggingService.LogError("Error retrieving tour prices by tour ID", ex, new { tourId });
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("stats")]
        public async Task<ActionResult<object>> GetPriceStats(
            [FromQuery] int? tourId = null,
            [FromQuery] DateTime? startDate = null,
            [FromQuery] DateTime? endDate = null)
        {
            try
            {
                var stats = await _service.GetPriceStatisticsAsync(tourId, startDate, endDate);
                _loggingService.LogInformation("Tour price statistics retrieved", new { tourId });
                return Ok(stats);
            }
            catch (Exception ex)
            {
                _loggingService.LogError("Error retrieving tour price statistics", ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<ActionResult<TourPrice>> Create(TourPrice entity)
        {
            try
            {
                var createdEntity = await _service.CreateAsync(entity);
                _loggingService.LogInformation("Tour price created", new { priceId = createdEntity.Id, tourId = createdEntity.TourId });
                return CreatedAtAction(nameof(GetById), new { id = createdEntity.Id }, createdEntity);
            }
            catch (Exception ex)
            {
                _loggingService.LogError("Error creating tour price", ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, TourPrice entity)
        {
            if (id != entity.Id)
                return BadRequest();

            if (!await _service.ExistsAsync(id))
                return NotFound();

            try
            {
                await _service.UpdateAsync(entity);
                _loggingService.LogInformation("Tour price updated", new { priceId = id });
                return NoContent();
            }
            catch (Exception ex)
            {
                _loggingService.LogError("Error updating tour price", ex, new { priceId = id });
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (!await _service.ExistsAsync(id))
                return NotFound();

            try
            {
                await _service.DeleteAsync(id);
                _loggingService.LogInformation("Tour price deleted", new { priceId = id });
                return NoContent();
            }
            catch (Exception ex)
            {
                _loggingService.LogError("Error deleting tour price", ex, new { priceId = id });
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
