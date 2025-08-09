using Microsoft.AspNetCore.Mvc;
using SD_Turizm.Application.Services;
using SD_Turizm.Core.Entities;

namespace SD_Turizm.API.Controllers.V2
{
    [ApiController]
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class CruiseController : ControllerBase
    {
        private readonly ICruiseService _service;
        private readonly ILoggingService _loggingService;

        public CruiseController(ICruiseService service, ILoggingService loggingService)
        {
            _service = service;
            _loggingService = loggingService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cruise>>> GetAll(
            [FromQuery] string? searchTerm = null,
            [FromQuery] string? destination = null,
            [FromQuery] bool? isActive = null)
        {
            try
            {
                var entities = await _service.GetAllAsync();
                var cruises = entities.ToList();

                // Apply filters
                if (!string.IsNullOrEmpty(searchTerm))
                    cruises = cruises.Where(c => c.Name?.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) == true || c.Code?.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) == true).ToList();
                if (!string.IsNullOrEmpty(destination))
                    cruises = cruises.Where(c => c.Description?.Contains(destination, StringComparison.OrdinalIgnoreCase) == true).ToList();
                if (isActive.HasValue)
                    cruises = cruises.Where(c => c.IsActive == isActive.Value).ToList();

                _loggingService.LogInformation("Cruises retrieved with filters", new { searchTerm, destination, isActive, count = cruises.Count });
                return Ok(cruises);
            }
            catch (Exception ex)
            {
                _loggingService.LogError("Error retrieving cruises", ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Cruise>> GetById(int id)
        {
            try
            {
                var entity = await _service.GetByIdAsync(id);
                if (entity == null)
                    return NotFound();

                _loggingService.LogInformation("Cruise retrieved by ID", new { cruiseId = id });
                return Ok(entity);
            }
            catch (Exception ex)
            {
                _loggingService.LogError("Error retrieving cruise by ID", ex, new { cruiseId = id });
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("code/{code}")]
        public async Task<ActionResult<Cruise>> GetByCode(string code)
        {
            try
            {
                var entities = await _service.GetAllAsync();
                var cruise = entities.FirstOrDefault(c => c.Code == code);
                
                if (cruise == null)
                    return NotFound();

                _loggingService.LogInformation("Cruise retrieved by code", new { code });
                return Ok(cruise);
            }
            catch (Exception ex)
            {
                _loggingService.LogError("Error retrieving cruise by code", ex, new { code });
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("statistics")]
        public async Task<ActionResult<object>> GetStatistics()
        {
            try
            {
                var entities = await _service.GetAllAsync();
                var cruises = entities.ToList();

                var stats = new
                {
                    TotalCruises = cruises.Count,
                    ActiveCruises = cruises.Count(c => c.IsActive),
                    CruisesByDestination = cruises.GroupBy(c => c.Description).Select(g => new { Destination = g.Key, Count = g.Count() }).ToList(),
                    TopCruises = cruises.OrderByDescending(c => c.IsActive).Take(10).ToList()
                };

                _loggingService.LogInformation("Cruise statistics retrieved");
                return Ok(stats);
            }
            catch (Exception ex)
            {
                _loggingService.LogError("Error retrieving cruise statistics", ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<ActionResult<Cruise>> Create(Cruise entity)
        {
            try
            {
                var createdEntity = await _service.CreateAsync(entity);
                _loggingService.LogInformation("Cruise created", new { cruiseId = createdEntity.Id, code = createdEntity.Code });
                return CreatedAtAction(nameof(GetById), new { id = createdEntity.Id }, createdEntity);
            }
            catch (Exception ex)
            {
                _loggingService.LogError("Error creating cruise", ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Cruise entity)
        {
            if (id != entity.Id)
                return BadRequest();

            try
            {
                if (!await _service.ExistsAsync(id))
                    return NotFound();

                await _service.UpdateAsync(entity);
                _loggingService.LogInformation("Cruise updated", new { cruiseId = id });
                return NoContent();
            }
            catch (Exception ex)
            {
                _loggingService.LogError("Error updating cruise", ex, new { cruiseId = id });
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
                _loggingService.LogInformation("Cruise deleted", new { cruiseId = id });
                return NoContent();
            }
            catch (Exception ex)
            {
                _loggingService.LogError("Error deleting cruise", ex, new { cruiseId = id });
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
