using Microsoft.AspNetCore.Mvc;
using SD_Turizm.Application.Services;
using SD_Turizm.Core.Entities;
using SD_Turizm.Core.DTOs;

namespace SD_Turizm.API.Controllers.V2
{
    [ApiController]
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class SaleItemController : ControllerBase
    {
        private readonly ISaleItemService _service;
        private readonly ILoggingService _loggingService;

        public SaleItemController(ISaleItemService service, ILoggingService loggingService)
        {
            _service = service;
            _loggingService = loggingService;
        }

        [HttpGet]
        public async Task<ActionResult<PagedResult<SaleItem>>> GetAll(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20,
            [FromQuery] string? sortBy = "date",
            [FromQuery] string? sortOrder = "desc",
            [FromQuery] int? saleId = null,
            [FromQuery] string? productType = null,
            [FromQuery] decimal? minAmount = null,
            [FromQuery] decimal? maxAmount = null)
        {
            try
            {
                var paginationDto = new PaginationDto
                {
                    Page = page,
                    PageSize = pageSize
                };

                var result = await _service.GetSaleItemsWithPaginationAsync(
                    paginationDto, saleId, productType, minAmount, maxAmount);

                _loggingService.LogInformation("Sale items retrieved with pagination", new { page, pageSize, saleId });

                return Ok(result);
            }
            catch (Exception ex)
            {
                _loggingService.LogError("Error retrieving sale items with pagination", ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SaleItem>> GetById(int id)
        {
            try
            {
                var entity = await _service.GetByIdAsync(id);
                if (entity == null)
                    return NotFound();
                
                _loggingService.LogInformation("Sale item retrieved", new { itemId = id });
                return Ok(entity);
            }
            catch (Exception ex)
            {
                _loggingService.LogError("Error retrieving sale item", ex, new { itemId = id });
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("sale/{saleId}")]
        public async Task<ActionResult<IEnumerable<SaleItem>>> GetBySaleId(int saleId)
        {
            try
            {
                var items = await _service.GetItemsBySaleIdAsync(saleId);
                _loggingService.LogInformation("Sale items retrieved by sale ID", new { saleId });
                return Ok(items);
            }
            catch (Exception ex)
            {
                _loggingService.LogError("Error retrieving sale items by sale ID", ex, new { saleId });
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("stats")]
        public async Task<ActionResult<object>> GetSaleItemStats(
            [FromQuery] int? saleId = null,
            [FromQuery] string? productType = null)
        {
            try
            {
                var stats = await _service.GetSaleItemStatisticsAsync(saleId, productType);
                _loggingService.LogInformation("Sale item statistics retrieved", new { saleId, productType });
                return Ok(stats);
            }
            catch (Exception ex)
            {
                _loggingService.LogError("Error retrieving sale item statistics", ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<ActionResult<SaleItem>> Create(SaleItem entity)
        {
            try
            {
                var createdEntity = await _service.CreateAsync(entity);
                _loggingService.LogInformation("Sale item created", new { itemId = createdEntity.Id, saleId = createdEntity.SaleId });
                return CreatedAtAction(nameof(GetById), new { id = createdEntity.Id }, createdEntity);
            }
            catch (Exception ex)
            {
                _loggingService.LogError("Error creating sale item", ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, SaleItem entity)
        {
            if (id != entity.Id)
                return BadRequest();

            if (!await _service.ExistsAsync(id))
                return NotFound();

            try
            {
                await _service.UpdateAsync(entity);
                _loggingService.LogInformation("Sale item updated", new { itemId = id });
                return NoContent();
            }
            catch (Exception ex)
            {
                _loggingService.LogError("Error updating sale item", ex, new { itemId = id });
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
                _loggingService.LogInformation("Sale item deleted", new { itemId = id });
                return NoContent();
            }
            catch (Exception ex)
            {
                _loggingService.LogError("Error deleting sale item", ex, new { itemId = id });
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
