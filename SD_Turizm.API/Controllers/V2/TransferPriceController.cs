using Microsoft.AspNetCore.Mvc;
using SD_Turizm.Application.Services;
using SD_Turizm.Core.Entities.Prices;
using SD_Turizm.Core.DTOs;

namespace SD_Turizm.API.Controllers.V2
{
    [ApiController]
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class TransferPriceController : ControllerBase
    {
        private readonly ITransferPriceService _service;
        private readonly ILoggingService _loggingService;

        public TransferPriceController(ITransferPriceService service, ILoggingService loggingService)
        {
            _service = service;
            _loggingService = loggingService;
        }

        [HttpGet]
        public async Task<ActionResult<PagedResult<TransferPrice>>> GetAll(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20,
            [FromQuery] string? sortBy = "date",
            [FromQuery] string? sortOrder = "desc",
            [FromQuery] int? transferCompanyId = null,
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

                var result = await _service.GetTransferPricesWithPaginationAsync(
                    paginationDto, transferCompanyId, minPrice, maxPrice, startDate, endDate);

                _loggingService.LogInformation("Transfer prices retrieved with pagination", new { page, pageSize, transferCompanyId });

                return Ok(result);
            }
            catch (Exception ex)
            {
                _loggingService.LogError("Error retrieving transfer prices with pagination", ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TransferPrice>> GetById(int id)
        {
            try
            {
                var entity = await _service.GetByIdAsync(id);
                if (entity == null)
                    return NotFound();
                
                _loggingService.LogInformation("Transfer price retrieved", new { priceId = id });
                return Ok(entity);
            }
            catch (Exception ex)
            {
                _loggingService.LogError("Error retrieving transfer price", ex, new { priceId = id });
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("company/{transferCompanyId}")]
        public async Task<ActionResult<IEnumerable<TransferPrice>>> GetByTransferCompanyId(int transferCompanyId)
        {
            try
            {
                var prices = await _service.GetPricesByTransferCompanyIdAsync(transferCompanyId);
                _loggingService.LogInformation("Transfer prices retrieved by company ID", new { transferCompanyId });
                return Ok(prices);
            }
            catch (Exception ex)
            {
                _loggingService.LogError("Error retrieving transfer prices by company ID", ex, new { transferCompanyId });
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("stats")]
        public async Task<ActionResult<object>> GetPriceStats(
            [FromQuery] int? transferCompanyId = null,
            [FromQuery] DateTime? startDate = null,
            [FromQuery] DateTime? endDate = null)
        {
            try
            {
                var stats = await _service.GetPriceStatisticsAsync(transferCompanyId, startDate, endDate);
                _loggingService.LogInformation("Transfer price statistics retrieved", new { transferCompanyId });
                return Ok(stats);
            }
            catch (Exception ex)
            {
                _loggingService.LogError("Error retrieving transfer price statistics", ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<ActionResult<TransferPrice>> Create(TransferPrice entity)
        {
            try
            {
                var createdEntity = await _service.CreateAsync(entity);
                _loggingService.LogInformation("Transfer price created", new { priceId = createdEntity.Id, transferCompanyId = createdEntity.TransferCompanyId });
                return CreatedAtAction(nameof(GetById), new { id = createdEntity.Id }, createdEntity);
            }
            catch (Exception ex)
            {
                _loggingService.LogError("Error creating transfer price", ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, TransferPrice entity)
        {
            if (id != entity.Id)
                return BadRequest();

            if (!await _service.ExistsAsync(id))
                return NotFound();

            try
            {
                await _service.UpdateAsync(entity);
                _loggingService.LogInformation("Transfer price updated", new { priceId = id });
                return NoContent();
            }
            catch (Exception ex)
            {
                _loggingService.LogError("Error updating transfer price", ex, new { priceId = id });
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
                _loggingService.LogInformation("Transfer price deleted", new { priceId = id });
                return NoContent();
            }
            catch (Exception ex)
            {
                _loggingService.LogError("Error deleting transfer price", ex, new { priceId = id });
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
