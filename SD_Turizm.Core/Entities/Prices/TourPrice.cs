using System.ComponentModel.DataAnnotations;

namespace SD_Turizm.Core.Entities.Prices
{
    public class TourPrice : BasePrice
    {
        public int TourId { get; set; }
        public virtual Tour Tour { get; set; } = null!;
        
        public int? TourOperatorId { get; set; }
        public virtual TourOperator? TourOperator { get; set; }
    }
} 