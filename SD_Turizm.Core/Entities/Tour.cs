using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using SD_Turizm.Core.Entities.Prices;

namespace SD_Turizm.Core.Entities
{
    public class Tour : BaseEntity
    {
        [Required]
        [MaxLength(50)]
        public string Code { get; set; } = string.Empty;
        
        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = string.Empty;
        
        [MaxLength(100)]
        public string VehicleType { get; set; } = string.Empty;
        
        [MaxLength(200)]
        public string Region { get; set; } = string.Empty;
        
        public int TourOperatorId { get; set; }
        public virtual TourOperator TourOperator { get; set; } = null!;
        public virtual ICollection<TourPrice> TourPrices { get; set; } = new HashSet<TourPrice>();
        
        // Web view'lar için ek property'ler
        [MaxLength(1000)]
        public string Description { get; set; } = string.Empty;
        
        [MaxLength(100)]
        public string Duration { get; set; } = string.Empty;
    }
} 