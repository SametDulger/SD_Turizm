using System.ComponentModel.DataAnnotations;

namespace SD_Turizm.Core.Entities.Prices
{
    public class TransferPrice : BasePrice
    {
        public int TransferCompanyId { get; set; }
        public int TransferId { get; set; }
        public virtual TransferCompany TransferCompany { get; set; } = null!;
        
        [Required]
        [MaxLength(100)]
        public string Route { get; set; } = string.Empty; // IST-AYT gibi
    }
} 