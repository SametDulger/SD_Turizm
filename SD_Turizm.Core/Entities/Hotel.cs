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
        
        [MaxLength(200)]
        public string Location { get; set; } = string.Empty;
        
        public int Stars { get; set; } = 3;
        
        [MaxLength(500)]
        public string Amenities { get; set; } = string.Empty;
        
        [MaxLength(200)]
        public string Website { get; set; } = string.Empty;
        
        public int StarRating { get; set; } = 3;
        
        public int Capacity { get; set; } = 100;
        
        public decimal Price { get; set; } = 0;
        
        public virtual ICollection<HotelPrice> HotelPrices { get; set; }
        
        public Hotel()
        {
            HotelPrices = new HashSet<HotelPrice>();
        }
    }
} 