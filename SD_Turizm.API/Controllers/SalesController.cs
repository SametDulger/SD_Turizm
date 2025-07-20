using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SD_Turizm.Application.Services;
using SD_Turizm.Core.Entities;

namespace SD_Turizm.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SalesController : ControllerBase
    {
        private readonly ISaleService _saleService;

        public SalesController(ISaleService saleService)
        {
            _saleService = saleService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Sale>>> GetSales()
        {
            var sales = await _saleService.GetAllSalesAsync();
            return Ok(sales);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Sale>> GetSale(int id)
        {
            var sale = await _saleService.GetSaleByIdAsync(id);
            if (sale == null)
            {
                return NotFound();
            }
            return Ok(sale);
        }

        [HttpGet("pnr/{pnrNumber}")]
        public async Task<ActionResult<Sale>> GetSaleByPNR(string pnrNumber)
        {
            var sale = await _saleService.GetSaleByPNRAsync(pnrNumber);
            if (sale == null)
            {
                return NotFound();
            }
            return Ok(sale);
        }

        [HttpGet("date-range")]
        public async Task<ActionResult<IEnumerable<Sale>>> GetSalesByDateRange([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var sales = await _saleService.GetSalesByDateRangeAsync(startDate, endDate);
            return Ok(sales);
        }

        [HttpGet("agency/{agencyCode}")]
        public async Task<ActionResult<IEnumerable<Sale>>> GetSalesByAgency(string agencyCode)
        {
            var sales = await _saleService.GetSalesByAgencyAsync(agencyCode);
            return Ok(sales);
        }

        [HttpGet("cari/{cariCode}")]
        public async Task<ActionResult<IEnumerable<Sale>>> GetSalesByCariCode(string cariCode)
        {
            var sales = await _saleService.GetSalesByCariCodeAsync(cariCode);
            return Ok(sales);
        }

        [HttpPost]
        public async Task<ActionResult<Sale>> CreateSale(Sale sale)
        {
            if (await _saleService.PNRExistsAsync(sale.PNRNumber))
            {
                return BadRequest("PNR number already exists");
            }

            var createdSale = await _saleService.CreateSaleAsync(sale);
            return CreatedAtAction(nameof(GetSale), new { id = createdSale.Id }, createdSale);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSale(int id, Sale sale)
        {
            if (id != sale.Id)
            {
                return BadRequest();
            }

            if (!await _saleService.SaleExistsAsync(id))
            {
                return NotFound();
            }

            await _saleService.UpdateSaleAsync(sale);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSale(int id)
        {
            if (!await _saleService.SaleExistsAsync(id))
            {
                return NotFound();
            }

            await _saleService.DeleteSaleAsync(id);
            return NoContent();
        }
    }
} 