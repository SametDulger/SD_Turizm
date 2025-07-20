using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using SD_Turizm.Core.Entities.Prices;

namespace SD_Turizm.Core.Entities
{
    public class TourOperator : Vendor
    {
        public virtual ICollection<Tour> Tours { get; set; }
        public virtual ICollection<TourPrice> Prices { get; set; }
        
        public TourOperator()
        {
            Tours = new HashSet<Tour>();
            Prices = new HashSet<TourPrice>();
        }
    }
} 