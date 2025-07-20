using Microsoft.AspNetCore.Mvc;
using SD_Turizm.Application.Services;
using SD_Turizm.Core.Entities;

namespace SD_Turizm.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TourController : ControllerBase
    {
        private readonly ITourService _tourService;

        public TourController(ITourService tourService)
        {
            _tourService = tourService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Tour>>> GetAll()
        {
            var entities = await _tourService.GetAllAsync();
            return Ok(entities);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Tour>> GetById(int id)
        {
            var entity = await _tourService.GetByIdAsync(id);
            if (entity == null)
                return NotFound();
            return Ok(entity);
        }

        [HttpPost]
        public async Task<ActionResult<Tour>> Create(Tour entity)
        {
            var createdEntity = await _tourService.CreateAsync(entity);
            return CreatedAtAction(nameof(GetById), new { id = createdEntity.Id }, createdEntity);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Tour entity)
        {
            if (id != entity.Id)
                return BadRequest();

            var exists = await _tourService.ExistsAsync(id);
            if (!exists)
                return NotFound();

            await _tourService.UpdateAsync(entity);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var exists = await _tourService.ExistsAsync(id);
            if (!exists)
                return NotFound();

            await _tourService.DeleteAsync(id);
            return NoContent();
        }
    }
} 