using Microsoft.AspNetCore.Mvc;
using SD_Turizm.Application.Services;
using SD_Turizm.Core.Entities;
using SD_Turizm.Core.DTOs;

namespace SD_Turizm.API.Controllers.V2
{
    [ApiController]
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class PackageItemController : ControllerBase
    {
        private readonly IPackageItemService _packageItemService;
        private readonly ILoggingService _loggingService;

        public PackageItemController(IPackageItemService packageItemService, ILoggingService loggingService)
        {
            _packageItemService = packageItemService;
            _loggingService = loggingService;
        }

        [HttpGet]
        public async Task<ActionResult<PagedResult<PackageItem>>> GetAll(
            [FromQuery] int page = 1, [FromQuery] int pageSize = 20,
            [FromQuery] string? sortBy = "name", [FromQuery] string? sortOrder = "asc",
            [FromQuery] int? packageId = null, [FromQuery] string? itemType = null,
            [FromQuery] decimal? minPrice = null, [FromQuery] decimal? maxPrice = null)
        {
            try
            {
                _loggingService.LogInformation("Getting package items with pagination", new { page, pageSize, packageId, itemType, minPrice, maxPrice });
                
                var pagination = new PaginationDto { Page = page, PageSize = pageSize };
                var result = await _packageItemService.GetPackageItemsWithPaginationAsync(pagination, packageId, itemType, minPrice, maxPrice);
                
                return Ok(result);
            }
            catch (Exception ex)
            {
                _loggingService.LogError("Error getting package items", ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PackageItem>> GetById(int id)
        {
            try
            {
                _loggingService.LogInformation("Getting package item by ID", new { id });
                
                var entity = await _packageItemService.GetByIdAsync(id);
                if (entity == null)
                {
                    _loggingService.LogWarning("Package item not found", new { id });
                    return NotFound();
                }
                
                return Ok(entity);
            }
            catch (Exception ex)
            {
                _loggingService.LogError($"Error getting package item by ID: {id}", ex, new { id });
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("package/{packageId}")]
        public async Task<ActionResult<IEnumerable<PackageItem>>> GetByPackageId(int packageId)
        {
            try
            {
                _loggingService.LogInformation("Getting package items by package ID", new { packageId });
                
                var items = await _packageItemService.GetItemsByPackageIdAsync(packageId);
                return Ok(items);
            }
            catch (Exception ex)
            {
                _loggingService.LogError($"Error getting package items by package ID: {packageId}", ex, new { packageId });
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("stats")]
        public async Task<ActionResult<object>> GetPackageItemStats()
        {
            try
            {
                _loggingService.LogInformation("Getting package item statistics");
                
                var stats = await _packageItemService.GetPackageItemStatisticsAsync();
                return Ok(stats);
            }
            catch (Exception ex)
            {
                _loggingService.LogError("Error getting package item statistics", ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<ActionResult<PackageItem>> Create(PackageItem entity)
        {
            try
            {
                _loggingService.LogInformation("Creating new package item", new { entity.PackageId });
                
                var createdEntity = await _packageItemService.CreateAsync(entity);
                return CreatedAtAction(nameof(GetById), new { id = createdEntity.Id }, createdEntity);
            }
            catch (Exception ex)
            {
                _loggingService.LogError("Error creating package item", ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, PackageItem entity)
        {
            try
            {
                _loggingService.LogInformation("Updating package item", new { id });
                
                if (id != entity.Id)
                    return BadRequest();

                var exists = await _packageItemService.ExistsAsync(id);
                if (!exists)
                {
                    _loggingService.LogWarning("Package item not found for update", new { id });
                    return NotFound();
                }

                await _packageItemService.UpdateAsync(entity);
                return NoContent();
            }
            catch (Exception ex)
            {
                _loggingService.LogError($"Error updating package item: {id}", ex, new { id });
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                _loggingService.LogInformation("Deleting package item", new { id });
                
                var exists = await _packageItemService.ExistsAsync(id);
                if (!exists)
                {
                    _loggingService.LogWarning("Package item not found for deletion", new { id });
                    return NotFound();
                }

                await _packageItemService.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _loggingService.LogError($"Error deleting package item: {id}", ex, new { id });
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
