using System.ComponentModel.DataAnnotations;

namespace SD_Turizm.Core.Entities.Prices
{
    public class RentACarPrice : BasePrice
    {
        public int RentACarId { get; set; }
        public virtual RentACar RentACar { get; set; } = null!;
        
        [Required]
        [MaxLength(50)]
        public string CarType { get; set; } = string.Empty;
        
        public decimal DailyPrice { get; set; }
        public decimal WeeklyPrice { get; set; }
        public decimal MonthlyPrice { get; set; }
    }
} 