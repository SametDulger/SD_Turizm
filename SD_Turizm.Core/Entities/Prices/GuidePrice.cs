using System.ComponentModel.DataAnnotations;

namespace SD_Turizm.Core.Entities.Prices
{
    public class GuidePrice : BasePrice
    {
        public int GuideId { get; set; }
        public virtual Guide Guide { get; set; } = null!;
        
        public decimal DailyPrice { get; set; }
        public decimal HalfDayPrice { get; set; }
    }
} 