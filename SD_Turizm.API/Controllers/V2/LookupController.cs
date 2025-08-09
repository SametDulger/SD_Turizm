using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SD_Turizm.Infrastructure.Data;

namespace SD_Turizm.API.Controllers.V2
{
    [ApiController]
    [Route("api/v2/[controller]")]
    public class LookupController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public LookupController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("currencies")]
        public async Task<IActionResult> GetCurrencies()
        {
            var currencies = await _context.Currencies
                .Where(c => c.IsActive)
                .OrderBy(c => c.DisplayOrder)
                .Select(c => new { c.Id, c.Code, c.Name, c.Symbol, c.Flag })
                .ToListAsync();

            return Ok(currencies);
        }

        [HttpGet("room-types")]
        public async Task<IActionResult> GetRoomTypes()
        {
            var roomTypes = await _context.RoomTypes
                .Where(rt => rt.IsActive)
                .OrderBy(rt => rt.DisplayOrder)
                .Select(rt => new { rt.Id, rt.Code, rt.Name, rt.Description, rt.Capacity })
                .ToListAsync();

            return Ok(roomTypes);
        }

        [HttpGet("board-types")]
        public async Task<IActionResult> GetBoardTypes()
        {
            var boardTypes = await _context.BoardTypes
                .Where(bt => bt.IsActive)
                .OrderBy(bt => bt.DisplayOrder)
                .Select(bt => new { bt.Id, bt.Code, bt.Name, bt.Description })
                .ToListAsync();

            return Ok(boardTypes);
        }

        [HttpGet("vendor-types")]
        public async Task<IActionResult> GetVendorTypes()
        {
            var vendorTypes = await _context.VendorTypes
                .Where(vt => vt.IsActive)
                .OrderBy(vt => vt.DisplayOrder)
                .Select(vt => new { vt.Id, vt.Code, vt.Name, vt.Description })
                .ToListAsync();

            return Ok(vendorTypes);
        }

        [HttpGet("sale-statuses")]
        public async Task<IActionResult> GetSaleStatuses()
        {
            var saleStatuses = await _context.SaleStatuses
                .Where(ss => ss.IsActive)
                .OrderBy(ss => ss.DisplayOrder)
                .Select(ss => new { ss.Id, ss.Code, ss.Name, ss.Description, ss.Color })
                .ToListAsync();

            return Ok(saleStatuses);
        }

        [HttpGet("person-types")]
        public async Task<IActionResult> GetPersonTypes()
        {
            var personTypes = await _context.PersonTypes
                .Where(pt => pt.IsActive)
                .OrderBy(pt => pt.DisplayOrder)
                .Select(pt => new { pt.Id, pt.Code, pt.Name, pt.Description, pt.MinAge, pt.MaxAge })
                .ToListAsync();

            return Ok(personTypes);
        }

        [HttpGet("room-locations")]
        public async Task<IActionResult> GetRoomLocations()
        {
            var roomLocations = await _context.RoomLocations
                .Where(rl => rl.IsActive)
                .OrderBy(rl => rl.DisplayOrder)
                .Select(rl => new { rl.Id, rl.Code, rl.Name, rl.Description })
                .ToListAsync();

            return Ok(roomLocations);
        }

        [HttpGet("product-types")]
        public async Task<IActionResult> GetProductTypes()
        {
            var productTypes = await _context.ProductTypes
                .Where(pt => pt.IsActive)
                .OrderBy(pt => pt.DisplayOrder)
                .Select(pt => new { pt.Id, pt.Code, pt.Name, pt.Description, pt.Icon })
                .ToListAsync();

            return Ok(productTypes);
        }

        [HttpGet("roles")]
        public async Task<IActionResult> GetRoles()
        {
            var roles = await _context.Roles
                .Where(r => r.IsActive)
                .Select(r => new { r.Id, r.Name, r.Description })
                .ToListAsync();

            return Ok(roles);
        }

        [HttpGet("transaction-types")]
        public async Task<IActionResult> GetTransactionTypes()
        {
            var transactionTypes = await _context.TransactionTypes
                .Where(tt => tt.IsActive)
                .OrderBy(tt => tt.DisplayOrder)
                .Select(tt => new { tt.Id, tt.Code, tt.Name, tt.Description })
                .ToListAsync();

            return Ok(transactionTypes);
        }
    }
}
