using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        public virtual Vendor? Vendor { get; set; }
        public string VendorType { get; set; } = string.Empty; // Hotel, TourOperator, etc.
        
        // Web view'lar için ek property'ler
        [MaxLength(50)]
        public string ItemType { get; set; } = string.Empty;
        
        public int ItemId { get; set; }
        
        public int Quantity { get; set; } = 1;
        
        [Column(TypeName = "decimal(18,2)")]
        public decimal UnitPrice { get; set; }
        
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalPrice { get; set; }
        
        [MaxLength(3)]
        public string Currency { get; set; } = "EUR";
    }
} 