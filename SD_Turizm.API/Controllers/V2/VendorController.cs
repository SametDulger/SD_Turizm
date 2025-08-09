using Microsoft.AspNetCore.Mvc;
using SD_Turizm.Application.Services;
using SD_Turizm.Core.DTOs;

namespace SD_Turizm.API.Controllers.V2
{
    [ApiController]
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class VendorController : ControllerBase
    {
        private readonly IVendorService _vendorService;

        public VendorController(IVendorService vendorService)
        {
            _vendorService = vendorService;
        }

        [HttpGet]
        public async Task<ActionResult<PagedResult<VendorDto>>> GetAll(
            [FromQuery] int page = 1, 
            [FromQuery] int pageSize = 10,
            [FromQuery] string? searchTerm = null,
            [FromQuery] string? vendorType = null,
            [FromQuery] bool? isActive = null)
        {
            var vendors = await _vendorService.GetVendorsWithPaginationAsync(page, pageSize, searchTerm, vendorType, isActive);
            return Ok(vendors);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<VendorDto>> GetById(int id)
        {
            var vendor = await _vendorService.GetVendorByIdAsync(id);
            if (vendor == null)
                return NotFound();
            return Ok(vendor);
        }

        [HttpGet("active")]
        public async Task<ActionResult<IEnumerable<VendorDto>>> GetActiveVendors()
        {
            var vendors = await _vendorService.GetActiveVendorsAsync();
            return Ok(vendors);
        }

        [HttpGet("type/{vendorType}")]
        public async Task<ActionResult<IEnumerable<VendorDto>>> GetByType(string vendorType)
        {
            var vendors = await _vendorService.GetVendorsByTypeAsync(vendorType);
            return Ok(vendors);
        }

        [HttpGet("stats")]
        public async Task<ActionResult<object>> GetVendorStats()
        {
            var stats = await _vendorService.GetVendorStatisticsAsync();
            return Ok(stats);
        }

        [HttpPost]
        public async Task<ActionResult<VendorDto>> Create(VendorDto vendorDto)
        {
            var createdVendor = await _vendorService.CreateVendorAsync(vendorDto);
            return CreatedAtAction(nameof(GetById), new { id = createdVendor.Id }, createdVendor);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<VendorDto>> Update(int id, VendorDto vendorDto)
        {
            if (id != vendorDto.Id)
                return BadRequest();

            var updatedVendor = await _vendorService.UpdateVendorAsync(vendorDto);
            if (updatedVendor == null)
                return NotFound();

            return Ok(updatedVendor);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var result = await _vendorService.DeleteVendorAsync(id);
            if (!result)
                return NotFound();

            return NoContent();
        }

        [HttpPatch("{id}/toggle-status")]
        public async Task<ActionResult<VendorDto>> ToggleStatus(int id)
        {
            var vendor = await _vendorService.ToggleVendorStatusAsync(id);
            if (vendor == null)
                return NotFound();

            return Ok(vendor);
        }
    }
}
