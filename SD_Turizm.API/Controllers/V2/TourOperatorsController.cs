using Microsoft.AspNetCore.Mvc;
using SD_Turizm.Application.Services;
using SD_Turizm.Core.Entities;
using SD_Turizm.Core.DTOs;

namespace SD_Turizm.API.Controllers.V2
{
    [ApiController]
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class TourOperatorsController : ControllerBase
    {
        private readonly ITourOperatorService _tourOperatorService;
        private readonly ILoggingService _loggingService;

        public TourOperatorsController(ITourOperatorService tourOperatorService, ILoggingService loggingService)
        {
            _tourOperatorService = tourOperatorService;
            _loggingService = loggingService;
        }

        [HttpGet]
        public async Task<ActionResult<PagedResult<TourOperator>>> GetAll(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20,
            [FromQuery] string? sortBy = "name",
            [FromQuery] string? sortOrder = "asc",
            [FromQuery] string? searchTerm = null,
            [FromQuery] string? region = null,
            [FromQuery] bool? isActive = null)
        {
            try
            {
                var paginationDto = new PaginationDto
                {
                    Page = page,
                    PageSize = pageSize
                };

                var result = await _tourOperatorService.GetTourOperatorsWithPaginationAsync(
                    paginationDto, searchTerm, region, isActive);

                _loggingService.LogInformation("Tour operators retrieved with pagination", new { page, pageSize, filters = new { searchTerm, region } });

                return Ok(result);
            }
            catch (Exception ex)
            {
                _loggingService.LogError("Error retrieving tour operators with pagination", ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TourOperator>> GetById(int id)
        {
            try
            {
                var tourOperator = await _tourOperatorService.GetTourOperatorByIdAsync(id);
                if (tourOperator == null)
                    return NotFound();
                
                _loggingService.LogInformation("Tour operator retrieved", new { tourOperatorId = id });
                return Ok(tourOperator);
            }
            catch (Exception ex)
            {
                _loggingService.LogError("Error retrieving tour operator", ex, new { tourOperatorId = id });
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("code/{code}")]
        public async Task<ActionResult<TourOperator>> GetByCode(string code)
        {
            try
            {
                var tourOperator = await _tourOperatorService.GetTourOperatorByCodeAsync(code);
                if (tourOperator == null)
                    return NotFound();
                
                _loggingService.LogInformation("Tour operator retrieved by code", new { code });
                return Ok(tourOperator);
            }
            catch (Exception ex)
            {
                _loggingService.LogError("Error retrieving tour operator by code", ex, new { code });
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("search")]
        public async Task<ActionResult<PagedResult<TourOperator>>> SearchTourOperators(
            [FromQuery] string region,
            [FromQuery] string? serviceType = null,
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

                var result = await _tourOperatorService.SearchTourOperatorsAsync(
                    paginationDto, region, serviceType);

                _loggingService.LogInformation("Tour operators searched", new { region, serviceType });
                return Ok(result);
            }
            catch (Exception ex)
            {
                _loggingService.LogError("Error searching tour operators", ex, new { region, serviceType });
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("stats")]
        public async Task<ActionResult<object>> GetTourOperatorStats(
            [FromQuery] string? groupBy = "region",
            [FromQuery] DateTime? startDate = null,
            [FromQuery] DateTime? endDate = null)
        {
            try
            {
                var stats = await _tourOperatorService.GetTourOperatorStatisticsAsync();
                _loggingService.LogInformation("Tour operator statistics retrieved", new { groupBy });
                return Ok(stats);
            }
            catch (Exception ex)
            {
                _loggingService.LogError("Error retrieving tour operator statistics", ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("bulk-update")]
        public async Task<IActionResult> BulkUpdate([FromBody] List<TourOperator> tourOperators)
        {
            try
            {
                var updatedCount = await _tourOperatorService.BulkUpdateAsync(tourOperators);
                _loggingService.LogInformation("Tour operators bulk updated", new { count = updatedCount });
                return Ok(new { updatedCount });
            }
            catch (Exception ex)
            {
                _loggingService.LogError("Error bulk updating tour operators", ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<ActionResult<TourOperator>> Create(TourOperator tourOperator)
        {
            try
            {
                if (await _tourOperatorService.TourOperatorCodeExistsAsync(tourOperator.Code))
                {
                    return BadRequest("Tour operator code already exists");
                }

                var createdTourOperator = await _tourOperatorService.CreateTourOperatorAsync(tourOperator);
                _loggingService.LogInformation("Tour operator created", new { tourOperatorId = createdTourOperator.Id, code = createdTourOperator.Code });
                return CreatedAtAction(nameof(GetById), new { id = createdTourOperator.Id }, createdTourOperator);
            }
            catch (Exception ex)
            {
                _loggingService.LogError("Error creating tour operator", ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, TourOperator tourOperator)
        {
            if (id != tourOperator.Id)
                return BadRequest();

            if (!await _tourOperatorService.TourOperatorExistsAsync(id))
                return NotFound();

            try
            {
                await _tourOperatorService.UpdateTourOperatorAsync(tourOperator);
                _loggingService.LogInformation("Tour operator updated", new { tourOperatorId = id });
                return NoContent();
            }
            catch (Exception ex)
            {
                _loggingService.LogError("Error updating tour operator", ex, new { tourOperatorId = id });
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (!await _tourOperatorService.TourOperatorExistsAsync(id))
                return NotFound();

            try
            {
                await _tourOperatorService.DeleteTourOperatorAsync(id);
                _loggingService.LogInformation("Tour operator deleted", new { tourOperatorId = id });
                return NoContent();
            }
            catch (Exception ex)
            {
                _loggingService.LogError("Error deleting tour operator", ex, new { tourOperatorId = id });
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
