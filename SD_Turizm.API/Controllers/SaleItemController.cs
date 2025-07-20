using Microsoft.AspNetCore.Mvc;
using SD_Turizm.Application.Services;
using SD_Turizm.Core.Entities;

namespace SD_Turizm.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SaleItemController : ControllerBase
    {
        private readonly ISaleItemService _saleItemService;

        public SaleItemController(ISaleItemService saleItemService)
        {
            _saleItemService = saleItemService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SaleItem>>> GetAll()
        {
            var entities = await _saleItemService.GetAllAsync();
            return Ok(entities);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SaleItem>> GetById(int id)
        {
            var entity = await _saleItemService.GetByIdAsync(id);
            if (entity == null)
                return NotFound();
            return Ok(entity);
        }

        [HttpPost]
        public async Task<ActionResult<SaleItem>> Create(SaleItem entity)
        {
            var createdEntity = await _saleItemService.CreateAsync(entity);
            return CreatedAtAction(nameof(GetById), new { id = createdEntity.Id }, createdEntity);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, SaleItem entity)
        {
            if (id != entity.Id)
                return BadRequest();

            var exists = await _saleItemService.ExistsAsync(id);
            if (!exists)
                return NotFound();

            await _saleItemService.UpdateAsync(entity);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var exists = await _saleItemService.ExistsAsync(id);
            if (!exists)
                return NotFound();

            await _saleItemService.DeleteAsync(id);
            return NoContent();
        }
    }
} 