using Microsoft.AspNetCore.Mvc;
using SD_Turizm.Application.Services;
using SD_Turizm.Core.Entities.Prices;

namespace SD_Turizm.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HotelPriceController : ControllerBase
    {
        private readonly IHotelPriceService _service;

        public HotelPriceController(IHotelPriceService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<HotelPrice>>> GetAll()
        {
            var entities = await _service.GetAllAsync();
            return Ok(entities);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<HotelPrice>> GetById(int id)
        {
            var entity = await _service.GetByIdAsync(id);
            if (entity == null)
                return NotFound();
            return Ok(entity);
        }

        [HttpPost]
        public async Task<ActionResult<HotelPrice>> Create(HotelPrice entity)
        {
            var createdEntity = await _service.CreateAsync(entity);
            return CreatedAtAction(nameof(GetById), new { id = createdEntity.Id }, createdEntity);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, HotelPrice entity)
        {
            if (id != entity.Id)
                return BadRequest();

            if (!await _service.ExistsAsync(id))
                return NotFound();

            await _service.UpdateAsync(entity);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (!await _service.ExistsAsync(id))
                return NotFound();

            await _service.DeleteAsync(id);
            return NoContent();
        }
    }
} 