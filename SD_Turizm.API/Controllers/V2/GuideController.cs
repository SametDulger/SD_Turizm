using Microsoft.AspNetCore.Mvc;
using SD_Turizm.Application.Services;
using SD_Turizm.Core.Entities;
using SD_Turizm.Core.DTOs;

namespace SD_Turizm.API.Controllers.V2
{
    [ApiController]
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class GuideController : ControllerBase
    {
        private readonly IGuideService _service;
        private readonly ILoggingService _loggingService;

        public GuideController(IGuideService service, ILoggingService loggingService)
        {
            _service = service;
            _loggingService = loggingService;
        }

        [HttpGet]
        public async Task<ActionResult<PagedResult<Guide>>> GetAll(
            [FromQuery] int page = 1, [FromQuery] int pageSize = 20,
            [FromQuery] string? sortBy = "name", [FromQuery] string? sortOrder = "asc",
            [FromQuery] string? searchTerm = null, [FromQuery] string? region = null,
            [FromQuery] bool? isActive = null)
        {
            try
            {
                _loggingService.LogInformation("Getting guides with pagination", new { page, pageSize, searchTerm, region, isActive });
                
                var pagination = new PaginationDto { Page = page, PageSize = pageSize };
                var result = await _service.GetGuidesWithPaginationAsync(pagination, searchTerm, region, isActive);
                
                return Ok(result);
            }
            catch (Exception ex)
            {
                _loggingService.LogError("Error getting guides", ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Guide>> GetById(int id)
        {
            try
            {
                _loggingService.LogInformation("Getting guide by ID", new { id });
                
                var entity = await _service.GetByIdAsync(id);
                if (entity == null)
                {
                    _loggingService.LogWarning("Guide not found", new { id });
                    return NotFound();
                }
                
                return Ok(entity);
            }
            catch (Exception ex)
            {
                _loggingService.LogError($"Error getting guide by ID: {id}", ex, new { id });
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("search")]
        public async Task<ActionResult<PagedResult<Guide>>> SearchGuides(
            [FromQuery] string searchTerm, [FromQuery] int page = 1, [FromQuery] int pageSize = 20,
            [FromQuery] string? language = null)
        {
            try
            {
                _loggingService.LogInformation("Searching guides", new { searchTerm, language, page, pageSize });
                
                var pagination = new PaginationDto { Page = page, PageSize = pageSize };
                var result = await _service.SearchGuidesAsync(pagination, searchTerm, language);
                
                return Ok(result);
            }
            catch (Exception ex)
            {
                _loggingService.LogError("Error searching guides", ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("stats")]
        public async Task<ActionResult<object>> GetGuideStats()
        {
            try
            {
                _loggingService.LogInformation("Getting guide statistics");
                
                var stats = await _service.GetGuideStatisticsAsync();
                return Ok(stats);
            }
            catch (Exception ex)
            {
                _loggingService.LogError("Error getting guide statistics", ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<ActionResult<Guide>> Create(Guide entity)
        {
            try
            {
                _loggingService.LogInformation("Creating new guide", new { entity.Name });
                
                var createdEntity = await _service.CreateAsync(entity);
                return CreatedAtAction(nameof(GetById), new { id = createdEntity.Id }, createdEntity);
            }
            catch (Exception ex)
            {
                _loggingService.LogError("Error creating guide", ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Guide entity)
        {
            try
            {
                _loggingService.LogInformation("Updating guide", new { id, entity.Name });
                
                if (id != entity.Id)
                    return BadRequest();

                if (!await _service.ExistsAsync(id))
                {
                    _loggingService.LogWarning("Guide not found for update", new { id });
                    return NotFound();
                }

                await _service.UpdateAsync(entity);
                return NoContent();
            }
            catch (Exception ex)
            {
                _loggingService.LogError($"Error updating guide: {id}", ex, new { id });
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                _loggingService.LogInformation("Deleting guide", new { id });
                
                if (!await _service.ExistsAsync(id))
                {
                    _loggingService.LogWarning("Guide not found for deletion", new { id });
                    return NotFound();
                }

                await _service.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _loggingService.LogError($"Error deleting guide: {id}", ex, new { id });
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
