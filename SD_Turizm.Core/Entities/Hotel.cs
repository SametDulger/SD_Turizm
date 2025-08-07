using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using SD_Turizm.Core.Entities.Prices;

namespace SD_Turizm.Core.Entities
{
    public class Hotel : Vendor
    {
        [MaxLength(100)]
        public string RoomLocations { get; set; } = string.Empty; // DNZ, GNL, PRK
        
        [MaxLength(100)]
        public string RoomTypes { get; set; } = string.Empty; // SGL, DBL, TRP, DBL+1, TRP+1
        
        public virtual ICollection<HotelPrice> HotelPrices { get; set; }
        
        public Hotel()
        {
            HotelPrices = new HashSet<HotelPrice>();
        }
    }
} 