using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using SD_Turizm.Core.Entities.Prices;

namespace SD_Turizm.Core.Entities
{
    public class Hotel : Vendor
    {
        [MaxLength(100)]
        public string RoomLocations { get; set; } // DNZ, GNL, PRK
        
        [MaxLength(100)]
        public string RoomTypes { get; set; } // SGL, DBL, TRP, DBL+1, TRP+1
        
        public virtual ICollection<HotelPrice> Prices { get; set; }
        
        public Hotel()
        {
            Prices = new HashSet<HotelPrice>();
        }
    }
} 