using System.ComponentModel.DataAnnotations;

namespace SD_Turizm.Core.Entities.Prices
{
    public class CruisePrice : BasePrice
    {
        public int CruiseId { get; set; }
        public virtual Cruise Cruise { get; set; } = null!;
        
        [Required]
        [MaxLength(10)]
        public string RoomType { get; set; } = string.Empty; // SGL, DBL, TRP, DBL+1, TRP+1
        
        [Required]
        [MaxLength(10)]
        public string RoomLocation { get; set; } = string.Empty; // DNZ, GNL, PRK
        
        [Required]
        [MaxLength(10)]
        public string BoardType { get; set; } = string.Empty; // OB, BB, HB, FB, AI, UAI
    }
} 