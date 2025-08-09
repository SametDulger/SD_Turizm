using Microsoft.AspNetCore.Mvc;
using SD_Turizm.Application.Services;
using SD_Turizm.Core.Entities;
using SD_Turizm.Core.DTOs;

namespace SD_Turizm.API.Controllers.V2
{
    [ApiController]
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class RentACarController : ControllerBase
    {
        private readonly IRentACarService _service;
        private readonly ILoggingService _loggingService;

        public RentACarController(IRentACarService service, ILoggingService loggingService)
        {
            _service = service;
            _loggingService = loggingService;
        }

        [HttpGet]
        public async Task<ActionResult<PagedResult<RentACar>>> GetAll(
            [FromQuery] int page = 1, [FromQuery] int pageSize = 20,
            [FromQuery] string? sortBy = "name", [FromQuery] string? sortOrder = "asc",
            [FromQuery] string? searchTerm = null, [FromQuery] string? location = null,
            [FromQuery] bool? isActive = null)
        {
            try
            {
                _loggingService.LogInformation("Getting rent a car companies with pagination", new { page, pageSize, searchTerm, location, isActive });
                
                var pagination = new PaginationDto { Page = page, PageSize = pageSize };
                var result = await _service.GetRentACarsWithPaginationAsync(pagination, searchTerm, location, isActive);
                
                return Ok(result);
            }
            catch (Exception ex)
            {
                _loggingService.LogError("Error getting rent a car companies", ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RentACar>> GetById(int id)
        {
            try
            {
                _loggingService.LogInformation("Getting rent a car company by ID", new { id });
                
                var entity = await _service.GetByIdAsync(id);
                if (entity == null)
                {
                    _loggingService.LogWarning("Rent a car company not found", new { id });
                    return NotFound();
                }
                
                return Ok(entity);
            }
            catch (Exception ex)
            {
                _loggingService.LogError($"Error getting rent a car company by ID: {id}", ex, new { id });
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("search")]
        public async Task<ActionResult<PagedResult<RentACar>>> SearchRentACars(
            [FromQuery] string searchTerm, [FromQuery] int page = 1, [FromQuery] int pageSize = 20,
            [FromQuery] string? serviceType = null)
        {
            try
            {
                _loggingService.LogInformation("Searching rent a car companies", new { searchTerm, serviceType, page, pageSize });
                
                var pagination = new PaginationDto { Page = page, PageSize = pageSize };
                var result = await _service.SearchRentACarsAsync(pagination, searchTerm, serviceType);
                
                return Ok(result);
            }
            catch (Exception ex)
            {
                _loggingService.LogError("Error searching rent a car companies", ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("stats")]
        public async Task<ActionResult<object>> GetRentACarStats()
        {
            try
            {
                _loggingService.LogInformation("Getting rent a car statistics");
                
                var stats = await _service.GetRentACarStatisticsAsync();
                return Ok(stats);
            }
            catch (Exception ex)
            {
                _loggingService.LogError("Error getting rent a car statistics", ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<ActionResult<RentACar>> Create(RentACar entity)
        {
            try
            {
                _loggingService.LogInformation("Creating new rent a car company", new { entity.Name });
                
                var createdEntity = await _service.CreateAsync(entity);
                return CreatedAtAction(nameof(GetById), new { id = createdEntity.Id }, createdEntity);
            }
            catch (Exception ex)
            {
                _loggingService.LogError("Error creating rent a car company", ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, RentACar entity)
        {
            try
            {
                _loggingService.LogInformation("Updating rent a car company", new { id, entity.Name });
                
                if (id != entity.Id)
                    return BadRequest();

                if (!await _service.ExistsAsync(id))
                {
                    _loggingService.LogWarning("Rent a car company not found for update", new { id });
                    return NotFound();
                }

                await _service.UpdateAsync(entity);
                return NoContent();
            }
            catch (Exception ex)
            {
                _loggingService.LogError($"Error updating rent a car company: {id}", ex, new { id });
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                _loggingService.LogInformation("Deleting rent a car company", new { id });
                
                if (!await _service.ExistsAsync(id))
                {
                    _loggingService.LogWarning("Rent a car company not found for deletion", new { id });
                    return NotFound();
                }

                await _service.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _loggingService.LogError($"Error deleting rent a car company: {id}", ex, new { id });
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
