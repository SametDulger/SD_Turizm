using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SD_Turizm.Core.Entities
{
    public class Package : BaseEntity
    {
        [Required]
        [MaxLength(50)]
        public string Code { get; set; }
        
        [Required]
        [MaxLength(200)]
        public string Name { get; set; }
        
        [MaxLength(500)]
        public string Description { get; set; }
        
        public decimal PackagePrice { get; set; }
        
        [Required]
        [MaxLength(3)]
        public string Currency { get; set; } = "EUR";
        
        public virtual ICollection<PackageItem> Items { get; set; }
        
        public Package()
        {
            Items = new HashSet<PackageItem>();
        }
    }
} 