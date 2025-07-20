using System.ComponentModel.DataAnnotations;

namespace SD_Turizm.Web.Models.DTOs
{
    public class PackageItemDto
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Paket ID zorunludur")]
        public int PackageId { get; set; }
        
        [Required(ErrorMessage = "Ürün adı zorunludur")]
        [StringLength(100, ErrorMessage = "Ürün adı en fazla 100 karakter olabilir")]
        public string ProductName { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Miktar zorunludur")]
        public int Quantity { get; set; }
        
        [Required(ErrorMessage = "Birim fiyat zorunludur")]
        [Range(0, double.MaxValue, ErrorMessage = "Birim fiyat 0'dan büyük olmalıdır")]
        public decimal UnitPrice { get; set; }
        
        public decimal TotalPrice => Quantity * UnitPrice;
        
        public string ItemType { get; set; } = string.Empty;
        public int ItemId { get; set; }
        
        public int Sequence { get; set; }
        public string Notes { get; set; } = string.Empty;
        
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int Order { get; set; }
    }
} 