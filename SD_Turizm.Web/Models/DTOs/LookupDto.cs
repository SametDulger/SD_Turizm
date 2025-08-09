using System.ComponentModel.DataAnnotations;

namespace SD_Turizm.Web.Models.DTOs
{
    public class LookupDto
    {
        public int Id { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;
        
        [MaxLength(200)]
        public string Description { get; set; } = string.Empty;
        
        public bool IsActive { get; set; } = true;
    }
    
    // Specific lookup DTOs
    public class CurrencyDto : LookupDto { }
    public class BoardTypeDto : LookupDto { }
    public class PersonTypeDto : LookupDto { }
    public class ProductTypeDto : LookupDto { }
    public class RoomLocationDto : LookupDto { }
    public class RoomTypeDto : LookupDto { }
    public class SaleStatusDto : LookupDto { }
    public class TransactionTypeDto : LookupDto { }
    public class VendorTypeDto : LookupDto { }
}
