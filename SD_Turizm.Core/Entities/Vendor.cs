using System.ComponentModel.DataAnnotations;

namespace SD_Turizm.Core.Entities
{
    public abstract class Vendor : BaseEntity
    {
        [Required]
        [MaxLength(50)]
        public string Code { get; set; } = string.Empty;
        
        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = string.Empty;
        
        [MaxLength(500)]
        public string Notes { get; set; } = string.Empty;
        
        [Required]
        [MaxLength(50)]
        public string CariCode { get; set; } = string.Empty;
        
        [MaxLength(20)]
        public string Phone { get; set; } = string.Empty;
        
        [MaxLength(100)]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        
        public int AddressId { get; set; }
        public virtual Address Address { get; set; } = null!;
    }
} 