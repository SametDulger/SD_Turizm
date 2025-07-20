using Microsoft.AspNetCore.Mvc;
using SD_Turizm.Application.Services;
using SD_Turizm.Core.Entities;

namespace SD_Turizm.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DashboardController : ControllerBase
    {
        private readonly ISaleService _saleService;
        private readonly ITourService _tourService;
        private readonly IHotelService _hotelService;
        private readonly ICariTransactionService _cariTransactionService;

        public DashboardController(
            ISaleService saleService,
            ITourService tourService,
            IHotelService hotelService,
            ICariTransactionService cariTransactionService)
        {
            _saleService = saleService;
            _tourService = tourService;
            _hotelService = hotelService;
            _cariTransactionService = cariTransactionService;
        }

        [HttpGet("statistics")]
        public async Task<ActionResult<object>> GetDashboardStatistics()
        {
            try
            {
                var sales = await _saleService.GetAllSalesAsync();
                var tours = await _tourService.GetAllAsync();
                var hotels = await _hotelService.GetAllHotelsAsync();
                var cariTransactions = await _cariTransactionService.GetAllAsync();

                var totalSales = sales != null ? sales.Sum(s => s.TotalAmount) : 0;
                var activeTours = tours != null ? tours.Count() : 0;
                var totalHotels = hotels != null ? hotels.Count() : 0;
                var totalCustomers = cariTransactions.Select(ct => ct.CariCode).Distinct().Count();

                var statistics = new
                {
                    TotalSales = totalSales,
                    ActiveTours = activeTours,
                    TotalHotels = totalHotels,
                    TotalCustomers = totalCustomers
                };

                return Ok(statistics);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }
    }
} 