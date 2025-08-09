using Microsoft.AspNetCore.Mvc;
using SD_Turizm.Application.Services;
using SD_Turizm.Core.Entities;
using SD_Turizm.Core.DTOs;

namespace SD_Turizm.API.Controllers.V2
{
    [ApiController]
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class AirlineController : ControllerBase
    {
        private readonly IAirlineService _service;
        private readonly ILoggingService _loggingService;

        public AirlineController(IAirlineService service, ILoggingService loggingService)
        {
            _service = service;
            _loggingService = loggingService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Airline>>> GetAll(
            [FromQuery] string? searchTerm = null,
            [FromQuery] string? country = null,
            [FromQuery] bool? isActive = null)
        {
            try
            {
                var entities = await _service.GetAllAsync();
                var airlines = entities.ToList();

                // Apply filters
                if (!string.IsNullOrEmpty(searchTerm))
                    airlines = airlines.Where(a => a.Name?.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) == true || a.Code?.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) == true).ToList();
                if (!string.IsNullOrEmpty(country))
                    airlines = airlines.Where(a => a.Country?.Contains(country, StringComparison.OrdinalIgnoreCase) == true).ToList();
                if (isActive.HasValue)
                    airlines = airlines.Where(a => a.IsActive == isActive.Value).ToList();

                _loggingService.LogInformation("Airlines retrieved with filters", new { searchTerm, country, isActive, count = airlines.Count });
                return Ok(airlines);
            }
            catch (Exception ex)
            {
                _loggingService.LogError("Error retrieving airlines", ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Airline>> GetById(int id)
        {
            try
            {
                var entity = await _service.GetByIdAsync(id);
                if (entity == null)
                    return NotFound();

                _loggingService.LogInformation("Airline retrieved by ID", new { airlineId = id });
                return Ok(entity);
            }
            catch (Exception ex)
            {
                _loggingService.LogError("Error retrieving airline by ID", ex, new { airlineId = id });
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("code/{code}")]
        public async Task<ActionResult<Airline>> GetByCode(string code)
        {
            try
            {
                var entities = await _service.GetAllAsync();
                var airline = entities.FirstOrDefault(a => a.Code == code);
                
                if (airline == null)
                    return NotFound();

                _loggingService.LogInformation("Airline retrieved by code", new { code });
                return Ok(airline);
            }
            catch (Exception ex)
            {
                _loggingService.LogError("Error retrieving airline by code", ex, new { code });
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("statistics")]
        public async Task<ActionResult<object>> GetStatistics()
        {
            try
            {
                var entities = await _service.GetAllAsync();
                var airlines = entities.ToList();

                var stats = new
                {
                    TotalAirlines = airlines.Count,
                    ActiveAirlines = airlines.Count(a => a.IsActive),
                    AirlinesByCountry = airlines.GroupBy(a => a.Country).Select(g => new { Country = g.Key, Count = g.Count() }).ToList(),
                    TopAirlines = airlines.OrderByDescending(a => a.IsActive).Take(10).ToList()
                };

                _loggingService.LogInformation("Airline statistics retrieved");
                return Ok(stats);
            }
            catch (Exception ex)
            {
                _loggingService.LogError("Error retrieving airline statistics", ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<ActionResult<Airline>> Create(Airline entity)
        {
            try
            {
                var createdEntity = await _service.CreateAsync(entity);
                _loggingService.LogInformation("Airline created", new { airlineId = createdEntity.Id, code = createdEntity.Code });
                return CreatedAtAction(nameof(GetById), new { id = createdEntity.Id }, createdEntity);
            }
            catch (Exception ex)
            {
                _loggingService.LogError("Error creating airline", ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Airline entity)
        {
            if (id != entity.Id)
                return BadRequest();

            try
            {
                if (!await _service.ExistsAsync(id))
                    return NotFound();

                await _service.UpdateAsync(entity);
                _loggingService.LogInformation("Airline updated", new { airlineId = id });
                return NoContent();
            }
            catch (Exception ex)
            {
                _loggingService.LogError("Error updating airline", ex, new { airlineId = id });
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                if (!await _service.ExistsAsync(id))
                    return NotFound();

                await _service.DeleteAsync(id);
                _loggingService.LogInformation("Airline deleted", new { airlineId = id });
                return NoContent();
            }
            catch (Exception ex)
            {
                _loggingService.LogError("Error deleting airline", ex, new { airlineId = id });
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
