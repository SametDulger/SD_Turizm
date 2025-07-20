using Microsoft.AspNetCore.Mvc;
using SD_Turizm.Application.Services;
using SD_Turizm.Core.Entities;

namespace SD_Turizm.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SalePersonController : ControllerBase
    {
        private readonly ISalePersonService _salePersonService;

        public SalePersonController(ISalePersonService salePersonService)
        {
            _salePersonService = salePersonService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SalePerson>>> GetAll()
        {
            var entities = await _salePersonService.GetAllAsync();
            return Ok(entities);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SalePerson>> GetById(int id)
        {
            var entity = await _salePersonService.GetByIdAsync(id);
            if (entity == null)
                return NotFound();
            return Ok(entity);
        }

        [HttpPost]
        public async Task<ActionResult<SalePerson>> Create(SalePerson entity)
        {
            var createdEntity = await _salePersonService.CreateAsync(entity);
            return CreatedAtAction(nameof(GetById), new { id = createdEntity.Id }, createdEntity);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, SalePerson entity)
        {
            if (id != entity.Id)
                return BadRequest();

            var exists = await _salePersonService.ExistsAsync(id);
            if (!exists)
                return NotFound();

            await _salePersonService.UpdateAsync(entity);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var exists = await _salePersonService.ExistsAsync(id);
            if (!exists)
                return NotFound();

            await _salePersonService.DeleteAsync(id);
            return NoContent();
        }
    }
} 