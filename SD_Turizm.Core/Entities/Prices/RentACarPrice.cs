using System.ComponentModel.DataAnnotations;

namespace SD_Turizm.Core.Entities.Prices
{
    public class RentACarPrice : BasePrice
    {
        public int RentACarId { get; set; }
        public decimal DailyPrice { get; set; }
        public decimal WeeklyPrice { get; set; }
        public decimal MonthlyPrice { get; set; }
        public virtual RentACar RentACar { get; set; } = null!;
        
        [Required]
        [MaxLength(100)]
        public string CarType { get; set; } = string.Empty;
    }
} 