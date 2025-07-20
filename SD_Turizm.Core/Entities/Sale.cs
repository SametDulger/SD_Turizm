using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SD_Turizm.Core.Entities
{
    public class Sale : BaseEntity
    {
        [Required]
        [MaxLength(20)]
        public string PNRNumber { get; set; }
        
        [Required]
        [MaxLength(50)]
        public string CariCode { get; set; }
        
        // Web view'lar için property'ler
        public string SaleNumber => PNRNumber;
        public DateTime SaleDate { get; set; } = DateTime.Now;
        public string CustomerName { get; set; } = string.Empty;
        public string Status { get; set; } = "Pending";
        
        [MaxLength(50)]
        public string AgencyCode { get; set; }
        
        [MaxLength(50)]
        public string PackageCode { get; set; }
        
        public bool IsPackageSale { get; set; }
        
        public decimal TotalAmount { get; set; }
        
        [Required]
        [MaxLength(3)]
        public string Currency { get; set; } = "EUR";
        
        [MaxLength(50)]
        public string SellerType { get; set; } = string.Empty;
        
        [MaxLength(50)]
        public string FileCode { get; set; } = string.Empty;
        
        public decimal SalePrice { get; set; }
        
        public decimal PurchasePrice { get; set; }
        
        [MaxLength(100)]
        public string ProductName { get; set; } = string.Empty;
        
        public decimal TotalAmountTL { get; set; }
        
        public virtual ICollection<SaleItem> Items { get; set; }
        public virtual ICollection<SalePerson> Persons { get; set; }
        
        public Sale()
        {
            Items = new HashSet<SaleItem>();
            Persons = new HashSet<SalePerson>();
        }
    }
} 