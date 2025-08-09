using Microsoft.AspNetCore.Mvc;
using SD_Turizm.Application.Services;
using SD_Turizm.Core.Entities;
using SD_Turizm.Core.DTOs;

namespace SD_Turizm.API.Controllers.V2
{
    [ApiController]
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class TransferCompanyController : ControllerBase
    {
        private readonly ITransferCompanyService _service;
        private readonly ILoggingService _loggingService;

        public TransferCompanyController(ITransferCompanyService service, ILoggingService loggingService)
        {
            _service = service;
            _loggingService = loggingService;
        }

        [HttpGet]
        public async Task<ActionResult<PagedResult<TransferCompany>>> GetAll(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20,
            [FromQuery] string? sortBy = "name",
            [FromQuery] string? sortOrder = "asc",
            [FromQuery] string? searchTerm = null,
            [FromQuery] string? region = null,
            [FromQuery] bool? isActive = null)
        {
            try
            {
                var paginationDto = new PaginationDto
                {
                    Page = page,
                    PageSize = pageSize
                };

                var result = await _service.GetTransferCompaniesWithPaginationAsync(
                    paginationDto, searchTerm, region, isActive);

                _loggingService.LogInformation("Transfer companies retrieved with pagination", new { page, pageSize, filters = new { searchTerm, region } });

                return Ok(result);
            }
            catch (Exception ex)
            {
                _loggingService.LogError("Error retrieving transfer companies with pagination", ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TransferCompany>> GetById(int id)
        {
            try
            {
                var entity = await _service.GetByIdAsync(id);
                if (entity == null)
                    return NotFound();
                
                _loggingService.LogInformation("Transfer company retrieved", new { companyId = id });
                return Ok(entity);
            }
            catch (Exception ex)
            {
                _loggingService.LogError("Error retrieving transfer company", ex, new { companyId = id });
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("search")]
        public async Task<ActionResult<PagedResult<TransferCompany>>> SearchTransferCompanies(
            [FromQuery] string region,
            [FromQuery] string? serviceType = null,
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

                var result = await _service.SearchTransferCompaniesAsync(
                    paginationDto, region, serviceType);

                _loggingService.LogInformation("Transfer companies searched", new { region, serviceType });
                return Ok(result);
            }
            catch (Exception ex)
            {
                _loggingService.LogError("Error searching transfer companies", ex, new { region, serviceType });
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("stats")]
        public async Task<ActionResult<object>> GetTransferCompanyStats(
            [FromQuery] string? groupBy = "region",
            [FromQuery] DateTime? startDate = null,
            [FromQuery] DateTime? endDate = null)
        {
            try
            {
                var stats = await _service.GetTransferCompanyStatisticsAsync();
                _loggingService.LogInformation("Transfer company statistics retrieved", new { groupBy });
                return Ok(stats);
            }
            catch (Exception ex)
            {
                _loggingService.LogError("Error retrieving transfer company statistics", ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<ActionResult<TransferCompany>> Create(TransferCompany entity)
        {
            try
            {
                var createdEntity = await _service.CreateAsync(entity);
                _loggingService.LogInformation("Transfer company created", new { companyId = createdEntity.Id, name = createdEntity.Name });
                return CreatedAtAction(nameof(GetById), new { id = createdEntity.Id }, createdEntity);
            }
            catch (Exception ex)
            {
                _loggingService.LogError("Error creating transfer company", ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, TransferCompany entity)
        {
            if (id != entity.Id)
                return BadRequest();

            if (!await _service.ExistsAsync(id))
                return NotFound();

            try
            {
                await _service.UpdateAsync(entity);
                _loggingService.LogInformation("Transfer company updated", new { companyId = id });
                return NoContent();
            }
            catch (Exception ex)
            {
                _loggingService.LogError("Error updating transfer company", ex, new { companyId = id });
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
                _loggingService.LogInformation("Transfer company deleted", new { companyId = id });
                return NoContent();
            }
            catch (Exception ex)
            {
                _loggingService.LogError("Error deleting transfer company", ex, new { companyId = id });
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
