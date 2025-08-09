using Microsoft.AspNetCore.Mvc;
using SD_Turizm.Application.Services;
using SD_Turizm.Core.Entities;

namespace SD_Turizm.API.Controllers.V1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class PackageItemController : ControllerBase
    {
        private readonly IPackageItemService _packageItemService;

        public PackageItemController(IPackageItemService packageItemService)
        {
            _packageItemService = packageItemService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PackageItem>>> GetAll()
        {
            var entities = await _packageItemService.GetAllAsync();
            return Ok(entities);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PackageItem>> GetById(int id)
        {
            var entity = await _packageItemService.GetByIdAsync(id);
            if (entity == null)
                return NotFound();
            return Ok(entity);
        }

        [HttpPost]
        public async Task<ActionResult<PackageItem>> Create(PackageItem entity)
        {
            var createdEntity = await _packageItemService.CreateAsync(entity);
            return CreatedAtAction(nameof(GetById), new { id = createdEntity.Id }, createdEntity);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, PackageItem entity)
        {
            if (id != entity.Id)
                return BadRequest();

            var exists = await _packageItemService.ExistsAsync(id);
            if (!exists)
                return NotFound();

            await _packageItemService.UpdateAsync(entity);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var exists = await _packageItemService.ExistsAsync(id);
            if (!exists)
                return NotFound();

            await _packageItemService.DeleteAsync(id);
            return NoContent();
        }
    }
}
