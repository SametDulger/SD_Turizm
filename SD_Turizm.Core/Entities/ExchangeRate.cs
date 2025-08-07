using System;
using System.ComponentModel.DataAnnotations;

namespace SD_Turizm.Core.Entities
{
    public class ExchangeRate : BaseEntity
    {
        public DateTime Date { get; set; }
        
        [Required]
        [MaxLength(3)]
        public string FromCurrency { get; set; } = string.Empty;
        
        [Required]
        [MaxLength(3)]
        public string ToCurrency { get; set; } = string.Empty;
        
        // Web view'lar için property'ler
        public string SourceCurrency => FromCurrency;
        public string TargetCurrency => ToCurrency;
        
        [MaxLength(3)]
        public string Currency { get; set; } = string.Empty;
        
        public decimal Rate { get; set; }
    }
} 