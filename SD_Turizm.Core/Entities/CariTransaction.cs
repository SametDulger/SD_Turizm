using System;
using System.ComponentModel.DataAnnotations;

namespace SD_Turizm.Core.Entities
{
    public class CariTransaction : BaseEntity
    {
        [Required]
        [MaxLength(50)]
        public string CariCode { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string CustomerName { get; set; } = string.Empty;
        
        public DateTime TransactionDate { get; set; }
        
        [Required]
        [MaxLength(10)]
        public string TransactionType { get; set; } // BORC, ALACAK
        
        public decimal Amount { get; set; }
        
        [Required]
        [MaxLength(3)]
        public string Currency { get; set; } = "EUR";
        
        public decimal AmountTL { get; set; }
        
        [MaxLength(200)]
        public string Description { get; set; }
        
        [MaxLength(20)]
        public string PNRNumber { get; set; }
        
        public int? SaleId { get; set; }
        public virtual Sale Sale { get; set; }
    }
} 