using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SD_Turizm.Application.Services;
using SD_Turizm.Core.Entities;

namespace SD_Turizm.API.Controllers.V1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class TourOperatorsController : ControllerBase
    {
        private readonly ITourOperatorService _tourOperatorService;

        public TourOperatorsController(ITourOperatorService tourOperatorService)
        {
            _tourOperatorService = tourOperatorService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TourOperator>>> GetTourOperators()
        {
            var tourOperators = await _tourOperatorService.GetAllTourOperatorsAsync();
            return Ok(tourOperators);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TourOperator>> GetTourOperator(int id)
        {
            var tourOperator = await _tourOperatorService.GetTourOperatorByIdAsync(id);
            if (tourOperator == null)
            {
                return NotFound();
            }
            return Ok(tourOperator);
        }

        [HttpGet("code/{code}")]
        public async Task<ActionResult<TourOperator>> GetTourOperatorByCode(string code)
        {
            var tourOperator = await _tourOperatorService.GetTourOperatorByCodeAsync(code);
            if (tourOperator == null)
            {
                return NotFound();
            }
            return Ok(tourOperator);
        }

        [HttpPost]
        public async Task<ActionResult<TourOperator>> CreateTourOperator(TourOperator tourOperator)
        {
            if (await _tourOperatorService.TourOperatorCodeExistsAsync(tourOperator.Code))
            {
                return BadRequest("Tour operator code already exists");
            }

            var createdTourOperator = await _tourOperatorService.CreateTourOperatorAsync(tourOperator);
            return CreatedAtAction(nameof(GetTourOperator), new { id = createdTourOperator.Id }, createdTourOperator);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTourOperator(int id, TourOperator tourOperator)
        {
            if (id != tourOperator.Id)
            {
                return BadRequest();
            }

            if (!await _tourOperatorService.TourOperatorExistsAsync(id))
            {
                return NotFound();
            }

            await _tourOperatorService.UpdateTourOperatorAsync(tourOperator);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTourOperator(int id)
        {
            if (!await _tourOperatorService.TourOperatorExistsAsync(id))
            {
                return NotFound();
            }

            await _tourOperatorService.DeleteTourOperatorAsync(id);
            return NoContent();
        }
    }
}
