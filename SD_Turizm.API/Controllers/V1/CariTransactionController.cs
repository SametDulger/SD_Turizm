using Microsoft.AspNetCore.Mvc;
using SD_Turizm.Application.Services;
using SD_Turizm.Core.Entities;

namespace SD_Turizm.API.Controllers.V1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class CariTransactionController : ControllerBase
    {
        private readonly ICariTransactionService _cariTransactionService;

        public CariTransactionController(ICariTransactionService cariTransactionService)
        {
            _cariTransactionService = cariTransactionService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CariTransaction>>> GetAll()
        {
            var transactions = await _cariTransactionService.GetAllAsync();
            return Ok(transactions);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CariTransaction>> GetById(int id)
        {
            var transaction = await _cariTransactionService.GetByIdAsync(id);
            if (transaction == null)
                return NotFound();
            return Ok(transaction);
        }

        [HttpPost]
        public async Task<ActionResult<CariTransaction>> Create(CariTransaction transaction)
        {
            var createdTransaction = await _cariTransactionService.CreateAsync(transaction);
            return CreatedAtAction(nameof(GetById), new { id = createdTransaction.Id }, createdTransaction);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, CariTransaction transaction)
        {
            if (id != transaction.Id)
                return BadRequest();

            if (!await _cariTransactionService.ExistsAsync(id))
                return NotFound();

            await _cariTransactionService.UpdateAsync(transaction);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (!await _cariTransactionService.ExistsAsync(id))
                return NotFound();

            await _cariTransactionService.DeleteAsync(id);
            return NoContent();
        }
    }
}
