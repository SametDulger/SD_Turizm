using System.ComponentModel.DataAnnotations;

namespace SD_Turizm.Core.Entities.Prices
{
    public class TransferPrice : BasePrice
    {
        public int TransferCompanyId { get; set; }
        public virtual TransferCompany TransferCompany { get; set; } = null!;
        
        public int TransferId { get; set; }
        
        [Required]
        [MaxLength(200)]
        public string Route { get; set; } = string.Empty;
    }
} 