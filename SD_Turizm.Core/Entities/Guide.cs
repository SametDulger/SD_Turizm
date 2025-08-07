using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using SD_Turizm.Core.Entities.Prices;

namespace SD_Turizm.Core.Entities
{
    public class Guide : Vendor
    {
        [MaxLength(200)]
        public string Languages { get; set; } = string.Empty;
        
        public virtual ICollection<GuidePrice> Prices { get; set; }
        
        public Guide()
        {
            Prices = new HashSet<GuidePrice>();
        }
    }
} 