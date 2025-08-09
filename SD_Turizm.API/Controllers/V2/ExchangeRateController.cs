using Microsoft.AspNetCore.Mvc;
using SD_Turizm.Application.Services;
using SD_Turizm.Core.Entities;
using SD_Turizm.Core.DTOs;

namespace SD_Turizm.API.Controllers.V2
{
    [ApiController]
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class ExchangeRateController : ControllerBase
    {
        private readonly IExchangeRateService _service;
        private readonly ILoggingService _loggingService;

        public ExchangeRateController(IExchangeRateService service, ILoggingService loggingService)
        {
            _service = service;
            _loggingService = loggingService;
        }

        [HttpGet]
        public async Task<ActionResult<PagedResult<ExchangeRate>>> GetAll(
            [FromQuery] int page = 1, [FromQuery] int pageSize = 20,
            [FromQuery] string? sortBy = "date", [FromQuery] string? sortOrder = "desc",
            [FromQuery] string? fromCurrency = null, [FromQuery] string? toCurrency = null,
            [FromQuery] DateTime? startDate = null, [FromQuery] DateTime? endDate = null)
        {
            try
            {
                _loggingService.LogInformation("Getting exchange rates with pagination", new { page, pageSize, fromCurrency, toCurrency, startDate, endDate });
                
                var pagination = new PaginationDto { Page = page, PageSize = pageSize };
                var result = await _service.GetExchangeRatesWithPaginationAsync(pagination, fromCurrency, toCurrency, startDate, endDate);
                
                return Ok(result);
            }
            catch (Exception ex)
            {
                _loggingService.LogError("Error getting exchange rates", ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ExchangeRate>> GetById(int id)
        {
            try
            {
                _loggingService.LogInformation("Getting exchange rate by ID", new { id });
                
                var entity = await _service.GetByIdAsync(id);
                if (entity == null)
                {
                    _loggingService.LogWarning("Exchange rate not found", new { id });
                    return NotFound();
                }
                
                return Ok(entity);
            }
            catch (Exception ex)
            {
                _loggingService.LogError($"Error getting exchange rate by ID: {id}", ex, new { id });
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("latest")]
        public async Task<ActionResult<ExchangeRate>> GetLatestRate([FromQuery] string fromCurrency, [FromQuery] string toCurrency)
        {
            try
            {
                _loggingService.LogInformation("Getting latest exchange rate", new { fromCurrency, toCurrency });
                
                var rate = await _service.GetLatestRateAsync(fromCurrency, toCurrency);
                if (rate == null)
                {
                    _loggingService.LogWarning("Latest exchange rate not found", new { fromCurrency, toCurrency });
                    return NotFound();
                }
                
                return Ok(rate);
            }
            catch (Exception ex)
            {
                _loggingService.LogError($"Error getting latest exchange rate: {fromCurrency} to {toCurrency}", ex, new { fromCurrency, toCurrency });
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("stats")]
        public async Task<ActionResult<object>> GetExchangeRateStats()
        {
            try
            {
                _loggingService.LogInformation("Getting exchange rate statistics");
                
                var stats = await _service.GetExchangeRateStatisticsAsync();
                return Ok(stats);
            }
            catch (Exception ex)
            {
                _loggingService.LogError("Error getting exchange rate statistics", ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<ActionResult<ExchangeRate>> Create(ExchangeRate entity)
        {
            try
            {
                _loggingService.LogInformation("Creating new exchange rate", new { entity.FromCurrency, entity.ToCurrency, entity.Rate });
                
                var createdEntity = await _service.CreateAsync(entity);
                return CreatedAtAction(nameof(GetById), new { id = createdEntity.Id }, createdEntity);
            }
            catch (Exception ex)
            {
                _loggingService.LogError("Error creating exchange rate", ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, ExchangeRate entity)
        {
            try
            {
                _loggingService.LogInformation("Updating exchange rate", new { id, entity.Rate });
                
                if (id != entity.Id)
                    return BadRequest();

                if (!await _service.ExistsAsync(id))
                {
                    _loggingService.LogWarning("Exchange rate not found for update", new { id });
                    return NotFound();
                }

                await _service.UpdateAsync(entity);
                return NoContent();
            }
            catch (Exception ex)
            {
                _loggingService.LogError($"Error updating exchange rate: {id}", ex, new { id });
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                _loggingService.LogInformation("Deleting exchange rate", new { id });
                
                if (!await _service.ExistsAsync(id))
                {
                    _loggingService.LogWarning("Exchange rate not found for deletion", new { id });
                    return NotFound();
                }

                await _service.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _loggingService.LogError($"Error deleting exchange rate: {id}", ex, new { id });
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
