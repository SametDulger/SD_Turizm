using Microsoft.AspNetCore.Mvc;
using SD_Turizm.Application.Services;
using SD_Turizm.Core.Entities;
using SD_Turizm.Core.DTOs;

namespace SD_Turizm.API.Controllers.V2
{
    [ApiController]
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class SalePersonController : ControllerBase
    {
        private readonly ISalePersonService _service;
        private readonly ILoggingService _loggingService;

        public SalePersonController(ISalePersonService service, ILoggingService loggingService)
        {
            _service = service;
            _loggingService = loggingService;
        }

        [HttpGet]
        public async Task<ActionResult<PagedResult<SalePerson>>> GetAll(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20,
            [FromQuery] string? sortBy = "name",
            [FromQuery] string? sortOrder = "asc",
            [FromQuery] string? searchTerm = null,
            [FromQuery] string? department = null,
            [FromQuery] bool? isActive = null)
        {
            try
            {
                var paginationDto = new PaginationDto
                {
                    Page = page,
                    PageSize = pageSize
                };

                var result = await _service.GetSalePersonsWithPaginationAsync(
                    paginationDto, searchTerm, department, isActive);

                _loggingService.LogInformation("Sale persons retrieved with pagination", new { page, pageSize, filters = new { searchTerm, department } });

                return Ok(result);
            }
            catch (Exception ex)
            {
                _loggingService.LogError("Error retrieving sale persons with pagination", ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SalePerson>> GetById(int id)
        {
            try
            {
                var entity = await _service.GetByIdAsync(id);
                if (entity == null)
                    return NotFound();
                
                _loggingService.LogInformation("Sale person retrieved", new { personId = id });
                return Ok(entity);
            }
            catch (Exception ex)
            {
                _loggingService.LogError("Error retrieving sale person", ex, new { personId = id });
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("search")]
        public async Task<ActionResult<PagedResult<SalePerson>>> SearchSalePersons(
            [FromQuery] string department,
            [FromQuery] string? position = null,
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

                var result = await _service.SearchSalePersonsAsync(
                    paginationDto, department, position);

                _loggingService.LogInformation("Sale persons searched", new { department, position });
                return Ok(result);
            }
            catch (Exception ex)
            {
                _loggingService.LogError("Error searching sale persons", ex, new { department, position });
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("stats")]
        public async Task<ActionResult<object>> GetSalePersonStats(
            [FromQuery] string? groupBy = "department",
            [FromQuery] DateTime? startDate = null,
            [FromQuery] DateTime? endDate = null)
        {
            try
            {
                var stats = await _service.GetSalePersonStatisticsAsync();
                _loggingService.LogInformation("Sale person statistics retrieved", new { groupBy });
                return Ok(stats);
            }
            catch (Exception ex)
            {
                _loggingService.LogError("Error retrieving sale person statistics", ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<ActionResult<SalePerson>> Create(SalePerson entity)
        {
            try
            {
                var createdEntity = await _service.CreateAsync(entity);
                _loggingService.LogInformation("Sale person created", new { personId = createdEntity.Id });
                return CreatedAtAction(nameof(GetById), new { id = createdEntity.Id }, createdEntity);
            }
            catch (Exception ex)
            {
                _loggingService.LogError("Error creating sale person", ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, SalePerson entity)
        {
            if (id != entity.Id)
                return BadRequest();

            if (!await _service.ExistsAsync(id))
                return NotFound();

            try
            {
                await _service.UpdateAsync(entity);
                _loggingService.LogInformation("Sale person updated", new { personId = id });
                return NoContent();
            }
            catch (Exception ex)
            {
                _loggingService.LogError("Error updating sale person", ex, new { personId = id });
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
                _loggingService.LogInformation("Sale person deleted", new { personId = id });
                return NoContent();
            }
            catch (Exception ex)
            {
                _loggingService.LogError("Error deleting sale person", ex, new { personId = id });
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
