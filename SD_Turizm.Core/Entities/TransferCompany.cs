using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using SD_Turizm.Core.Entities.Prices;

namespace SD_Turizm.Core.Entities
{
    public class TransferCompany : Vendor
    {
        [MaxLength(200)]
        public string Routes { get; set; } // Transfer güzergahları
        
        [MaxLength(100)]
        public string VehicleType { get; set; }
        
        [MaxLength(200)]
        public string Region { get; set; }
        
        [MaxLength(500)]
        public string Description { get; set; } = string.Empty;
        
        public virtual ICollection<TransferPrice> Prices { get; set; }
        
        public TransferCompany()
        {
            Prices = new HashSet<TransferPrice>();
        }
    }
} 