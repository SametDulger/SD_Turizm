using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SD_Turizm.Core.Entities
{
    public class Package : BaseEntity
    {
        [Required]
        [MaxLength(50)]
        public string Code { get; set; } = string.Empty;
        
        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = string.Empty;
        
        [MaxLength(500)]
        public string Description { get; set; } = string.Empty;
        
        public decimal PackagePrice { get; set; }
        
        [Required]
        [MaxLength(3)]
        public string Currency { get; set; } = "EUR";
        
        public virtual ICollection<PackageItem> PackageItems { get; set; }
        
        public Package()
        {
            PackageItems = new HashSet<PackageItem>();
        }
    }
} 