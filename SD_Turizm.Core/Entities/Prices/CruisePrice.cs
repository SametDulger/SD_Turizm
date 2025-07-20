using System.ComponentModel.DataAnnotations;

namespace SD_Turizm.Core.Entities.Prices
{
    public class CruisePrice : BasePrice
    {
        public int CruiseId { get; set; }
        public virtual Cruise Cruise { get; set; }
        
        [Required]
        [MaxLength(10)]
        public string RoomType { get; set; } // SGL, DBL, TRP, DBL+1, TRP+1
        
        [Required]
        [MaxLength(10)]
        public string RoomLocation { get; set; } // DNZ, GNL, PRK
        
        [Required]
        [MaxLength(10)]
        public string BoardType { get; set; } // OB, BB, HB, FB, AI, UAI
    }
} 