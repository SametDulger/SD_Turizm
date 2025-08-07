using System.ComponentModel.DataAnnotations;

namespace SD_Turizm.Core.Entities.Prices
{
    public class AirlinePrice : BasePrice
    {
        public int AirlineId { get; set; }
        public virtual Airline Airline { get; set; } = null!;
        
        [Required]
        [MaxLength(100)]
        public string Route { get; set; } = string.Empty;
    }
} 