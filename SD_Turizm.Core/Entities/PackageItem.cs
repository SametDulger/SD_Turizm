using System;
using System.ComponentModel.DataAnnotations;

namespace SD_Turizm.Core.Entities
{
    public class PackageItem : BaseEntity
    {
        public int PackageId { get; set; }
        public virtual Package Package { get; set; } = null!;
        
        public DateTime Date { get; set; }
        
        [Required]
        [MaxLength(10)]
        public string ServiceType { get; set; } = string.Empty; // HTL, TUR, TRA, TRD, RCR, GEM
        
        [MaxLength(200)]
        public string Description { get; set; } = string.Empty;
        
        public int? VendorId { get; set; }
        public string VendorType { get; set; } = string.Empty; // Hotel, TourOperator, etc.
        
        // Web view'lar için ek property'ler
        [MaxLength(50)]
        public string ItemType { get; set; } = string.Empty;
        
        public int ItemId { get; set; }
    }
} 