using Microsoft.AspNetCore.Mvc;
using SD_Turizm.Application.Services;
using SD_Turizm.Core.Entities;
using SD_Turizm.Core.DTOs;

namespace SD_Turizm.API.Controllers.V2
{
    [ApiController]
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class PackageController : ControllerBase
    {
        private readonly IPackageService _service;
        private readonly ILoggingService _loggingService;

        public PackageController(IPackageService service, ILoggingService loggingService)
        {
            _service = service;
            _loggingService = loggingService;
        }

        [HttpGet]
        public async Task<ActionResult<PagedResult<Package>>> GetAll(
            [FromQuery] int page = 1, [FromQuery] int pageSize = 20,
            [FromQuery] string? sortBy = "name", [FromQuery] string? sortOrder = "asc",
            [FromQuery] string? searchTerm = null, [FromQuery] string? destination = null,
            [FromQuery] bool? isActive = null)
        {
            try
            {
                _loggingService.LogInformation("Getting packages with pagination", new { page, pageSize, searchTerm, destination, isActive });
                
                var pagination = new PaginationDto { Page = page, PageSize = pageSize };
                var result = await _service.GetPackagesWithPaginationAsync(pagination, searchTerm, destination, isActive);
                
                return Ok(result);
            }
            catch (Exception ex)
            {
                _loggingService.LogError("Error getting packages", ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Package>> GetById(int id)
        {
            try
            {
                _loggingService.LogInformation("Getting package by ID", new { id });
                
                var entity = await _service.GetByIdAsync(id);
                if (entity == null)
                {
                    _loggingService.LogWarning("Package not found", new { id });
                    return NotFound();
                }
                
                return Ok(entity);
            }
            catch (Exception ex)
            {
                _loggingService.LogError($"Error getting package by ID: {id}", ex, new { id });
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("search")]
        public async Task<ActionResult<PagedResult<Package>>> SearchPackages(
            [FromQuery] string searchTerm, [FromQuery] int page = 1, [FromQuery] int pageSize = 20,
            [FromQuery] string? packageType = null)
        {
            try
            {
                _loggingService.LogInformation("Searching packages", new { searchTerm, packageType, page, pageSize });
                
                var pagination = new PaginationDto { Page = page, PageSize = pageSize };
                var result = await _service.SearchPackagesAsync(pagination, searchTerm, packageType);
                
                return Ok(result);
            }
            catch (Exception ex)
            {
                _loggingService.LogError("Error searching packages", ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("stats")]
        public async Task<ActionResult<object>> GetPackageStats()
        {
            try
            {
                _loggingService.LogInformation("Getting package statistics");
                
                var stats = await _service.GetPackageStatisticsAsync();
                return Ok(stats);
            }
            catch (Exception ex)
            {
                _loggingService.LogError("Error getting package statistics", ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<ActionResult<Package>> Create(Package entity)
        {
            try
            {
                _loggingService.LogInformation("Creating new package", new { entity.Name });
                
                var createdEntity = await _service.CreateAsync(entity);
                return CreatedAtAction(nameof(GetById), new { id = createdEntity.Id }, createdEntity);
            }
            catch (Exception ex)
            {
                _loggingService.LogError("Error creating package", ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Package entity)
        {
            try
            {
                _loggingService.LogInformation("Updating package", new { id, entity.Name });
                
                if (id != entity.Id)
                    return BadRequest();

                if (!await _service.ExistsAsync(id))
                {
                    _loggingService.LogWarning("Package not found for update", new { id });
                    return NotFound();
                }

                await _service.UpdateAsync(entity);
                return NoContent();
            }
            catch (Exception ex)
            {
                _loggingService.LogError($"Error updating package: {id}", ex, new { id });
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                _loggingService.LogInformation("Deleting package", new { id });
                
                if (!await _service.ExistsAsync(id))
                {
                    _loggingService.LogWarning("Package not found for deletion", new { id });
                    return NotFound();
                }

                await _service.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _loggingService.LogError($"Error deleting package: {id}", ex, new { id });
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
