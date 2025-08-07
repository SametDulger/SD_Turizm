using System;
using System.ComponentModel.DataAnnotations;

namespace SD_Turizm.Core.Entities
{
    public class SaleItem : BaseEntity
    {
        public int SaleId { get; set; }
        public virtual Sale Sale { get; set; } = null!;
        
        public DateTime Date { get; set; }
        
        [Required]
        [MaxLength(10)]
        public string ServiceType { get; set; } = string.Empty; // HTL, TUR, TRA, TRD, RCR, GEM
        
        [MaxLength(200)]
        public string Description { get; set; } = string.Empty;
        
        public int? VendorId { get; set; }
        public string? VendorType { get; set; }
        
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
        
        [Required]
        [MaxLength(3)]
        public string Currency { get; set; } = "EUR";
        
        // Web view'lar için ek property'ler
        [MaxLength(50)]
        public string? ItemType { get; set; }
        
        public int ItemId { get; set; }
        
        public int Quantity { get; set; }
        
        public decimal Price { get; set; }
    }
} 