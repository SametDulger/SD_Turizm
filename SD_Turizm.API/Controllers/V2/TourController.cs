using Microsoft.AspNetCore.Mvc;
using SD_Turizm.Application.Services;
using SD_Turizm.Core.Entities;
using SD_Turizm.Core.DTOs;

namespace SD_Turizm.API.Controllers.V2
{
    [ApiController]
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class TourController : ControllerBase
    {
        private readonly ITourService _tourService;
        private readonly ILoggingService _loggingService;

        public TourController(ITourService tourService, ILoggingService loggingService)
        {
            _tourService = tourService;
            _loggingService = loggingService;
        }

        [HttpGet]
        public async Task<ActionResult<PagedResult<Tour>>> GetAll(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20,
            [FromQuery] string? sortBy = "name",
            [FromQuery] string? sortOrder = "asc",
            [FromQuery] string? searchTerm = null,
            [FromQuery] string? destination = null,
            [FromQuery] int? duration = null,
            [FromQuery] bool? isActive = null)
        {
            try
            {
                var paginationDto = new PaginationDto
                {
                    Page = page,
                    PageSize = pageSize
                };

                var result = await _tourService.GetToursWithPaginationAsync(
                    paginationDto, searchTerm, destination, duration, isActive);

                _loggingService.LogInformation("Tours retrieved with pagination", new { page, pageSize, filters = new { searchTerm, destination, duration } });

                return Ok(result);
            }
            catch (Exception ex)
            {
                _loggingService.LogError("Error retrieving tours with pagination", ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Tour>> GetById(int id)
        {
            try
            {
                var tour = await _tourService.GetByIdAsync(id);
                if (tour == null)
                    return NotFound();
                
                _loggingService.LogInformation("Tour retrieved", new { tourId = id });
                return Ok(tour);
            }
            catch (Exception ex)
            {
                _loggingService.LogError("Error retrieving tour", ex, new { tourId = id });
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("code/{code}")]
        public async Task<ActionResult<Tour>> GetByCode(string code)
        {
            try
            {
                var tour = await _tourService.GetByCodeAsync(code);
                if (tour == null)
                    return NotFound();
                
                _loggingService.LogInformation("Tour retrieved by code", new { code });
                return Ok(tour);
            }
            catch (Exception ex)
            {
                _loggingService.LogError("Error retrieving tour by code", ex, new { code });
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("search")]
        public async Task<ActionResult<PagedResult<Tour>>> SearchTours(
            [FromQuery] string destination,
            [FromQuery] int? duration = null,
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

                var result = await _tourService.SearchToursAsync(
                    paginationDto, destination, duration, minPrice, maxPrice);

                _loggingService.LogInformation("Tours searched", new { destination, duration, minPrice, maxPrice });
                return Ok(result);
            }
            catch (Exception ex)
            {
                _loggingService.LogError("Error searching tours", ex, new { destination, duration });
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("stats")]
        public async Task<ActionResult<object>> GetTourStats(
            [FromQuery] string? groupBy = "destination",
            [FromQuery] DateTime? startDate = null,
            [FromQuery] DateTime? endDate = null)
        {
            try
            {
                var stats = await _tourService.GetTourStatisticsAsync();
                _loggingService.LogInformation("Tour statistics retrieved", new { groupBy });
                return Ok(stats);
            }
            catch (Exception ex)
            {
                _loggingService.LogError("Error retrieving tour statistics", ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("bulk-update")]
        public async Task<IActionResult> BulkUpdate([FromBody] List<Tour> tours)
        {
            try
            {
                var updatedCount = await _tourService.BulkUpdateAsync(tours);
                _loggingService.LogInformation("Tours bulk updated", new { count = updatedCount });
                return Ok(new { updatedCount });
            }
            catch (Exception ex)
            {
                _loggingService.LogError("Error bulk updating tours", ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<ActionResult<Tour>> Create(Tour tour)
        {
            try
            {
                var createdTour = await _tourService.CreateAsync(tour);
                _loggingService.LogInformation("Tour created", new { tourId = createdTour.Id, code = createdTour.Code });
                return CreatedAtAction(nameof(GetById), new { id = createdTour.Id }, createdTour);
            }
            catch (Exception ex)
            {
                _loggingService.LogError("Error creating tour", ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Tour tour)
        {
            if (id != tour.Id)
                return BadRequest();

            if (!await _tourService.ExistsAsync(id))
                return NotFound();

            try
            {
                await _tourService.UpdateAsync(tour);
                _loggingService.LogInformation("Tour updated", new { tourId = id });
                return NoContent();
            }
            catch (Exception ex)
            {
                _loggingService.LogError("Error updating tour", ex, new { tourId = id });
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (!await _tourService.ExistsAsync(id))
                return NotFound();

            try
            {
                await _tourService.DeleteAsync(id);
                _loggingService.LogInformation("Tour deleted", new { tourId = id });
                return NoContent();
            }
            catch (Exception ex)
            {
                _loggingService.LogError("Error deleting tour", ex, new { tourId = id });
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
