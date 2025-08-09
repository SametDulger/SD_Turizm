using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using SD_Turizm.Core.Entities.Prices;

namespace SD_Turizm.Core.Entities
{
    public class Cruise : Vendor
    {
        [MaxLength(100)]
        public string RoomLocations { get; set; } = string.Empty; // DNZ, GNL, PRK
        
        [MaxLength(100)]
        public string RoomTypes { get; set; } = string.Empty; // SGL, DBL, TRP, DBL+1, TRP+1
        

        
        public virtual ICollection<CruisePrice> Prices { get; set; }
        
        public Cruise()
        {
            Prices = new HashSet<CruisePrice>();
        }
    }
} 