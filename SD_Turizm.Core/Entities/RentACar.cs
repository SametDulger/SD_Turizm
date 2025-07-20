using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using SD_Turizm.Core.Entities.Prices;

namespace SD_Turizm.Core.Entities
{
    public class RentACar : Vendor
    {
        [MaxLength(500)]
        public string AvailableCars { get; set; }
        
        public virtual ICollection<RentACarPrice> Prices { get; set; }
        
        public RentACar()
        {
            Prices = new HashSet<RentACarPrice>();
        }
    }
} 