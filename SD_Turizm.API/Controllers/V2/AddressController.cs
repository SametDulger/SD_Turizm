using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SD_Turizm.Application.Services;
using SD_Turizm.Core.Entities;
using SD_Turizm.Core.DTOs;

namespace SD_Turizm.API.Controllers.V2
{
    [ApiController]
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Authorize]
    public class AddressController : ControllerBase
    {
        private readonly IAddressService _addressService;
        private readonly ILoggingService _loggingService;

        public AddressController(IAddressService addressService, ILoggingService loggingService)
        {
            _addressService = addressService;
            _loggingService = loggingService;
        }

        [HttpGet]
        public async Task<ActionResult<PagedResult<Address>>> GetAll(
            [FromQuery] PaginationDto pagination,
            [FromQuery] string? searchTerm = null,
            [FromQuery] string? city = null,
            [FromQuery] string? country = null)
        {
            try
            {
                var result = await _addressService.GetAddressesWithPaginationAsync(pagination, searchTerm, city, country);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _loggingService.LogError("Error getting addresses with pagination", ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Address>> GetById(int id)
        {
            try
            {
                var address = await _addressService.GetByIdAsync(id);
                if (address == null)
                    return NotFound();

                return Ok(address);
            }
            catch (Exception ex)
            {
                _loggingService.LogError($"Error getting address by id: {id}", ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("search")]
        public async Task<ActionResult<PagedResult<Address>>> SearchAddresses(
            [FromQuery] PaginationDto pagination,
            [FromQuery] string city,
            [FromQuery] string? country = null)
        {
            try
            {
                var result = await _addressService.SearchAddressesAsync(pagination, city, country);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _loggingService.LogError("Error searching addresses", ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("statistics")]
        public async Task<ActionResult<object>> GetStatistics()
        {
            try
            {
                var statistics = await _addressService.GetAddressStatisticsAsync();
                return Ok(statistics);
            }
            catch (Exception ex)
            {
                _loggingService.LogError("Error getting address statistics", ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<ActionResult<Address>> Create(Address address)
        {
            try
            {
                var createdAddress = await _addressService.CreateAsync(address);
                return CreatedAtAction(nameof(GetById), new { id = createdAddress.Id }, createdAddress);
            }
            catch (Exception ex)
            {
                _loggingService.LogError("Error creating address", ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Address address)
        {
            try
            {
                if (id != address.Id)
                    return BadRequest();

                var updatedAddress = await _addressService.UpdateAsync(address);
                return Ok(updatedAddress);
            }
            catch (Exception ex)
            {
                _loggingService.LogError($"Error updating address: {id}", ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _addressService.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _loggingService.LogError($"Error deleting address: {id}", ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("bulk-update")]
        public async Task<IActionResult> BulkUpdate([FromBody] List<Address> addresses)
        {
            try
            {
                var updatedAddresses = await _addressService.BulkUpdateAsync(addresses);
                return Ok(updatedAddresses);
            }
            catch (Exception ex)
            {
                _loggingService.LogError("Error bulk updating addresses", ex);
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
