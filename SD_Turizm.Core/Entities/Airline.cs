using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using SD_Turizm.Core.Entities.Prices;

namespace SD_Turizm.Core.Entities
{
    public class Airline : Vendor
    {
        [MaxLength(200)]
        public string AircraftTypes { get; set; } = string.Empty;
        
        [MaxLength(200)]
        public string FlightRegions { get; set; } = string.Empty;
        
        public virtual ICollection<AirlinePrice> Prices { get; set; }
        
        public Airline()
        {
            Prices = new HashSet<AirlinePrice>();
        }
    }
} 