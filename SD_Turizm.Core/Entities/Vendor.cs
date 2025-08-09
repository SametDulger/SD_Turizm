using System.ComponentModel.DataAnnotations;

namespace SD_Turizm.Core.Entities
{
    public class Vendor : BaseEntity
    {
        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = string.Empty;
        
        [Required]
        [MaxLength(50)]
        public string Code { get; set; } = string.Empty;
        
        [MaxLength(20)]
        public string Phone { get; set; } = string.Empty;
        
        [MaxLength(100)]
        public string Email { get; set; } = string.Empty;
        
        [MaxLength(100)]
        public string Country { get; set; } = string.Empty;
        

        
        [MaxLength(500)]
        public string Address { get; set; } = string.Empty;
        
        [MaxLength(1000)]
        public string Description { get; set; } = string.Empty;
        
        [MaxLength(50)]
        public string Type { get; set; } = string.Empty;
        
        [MaxLength(50)]
        public string CariCode { get; set; } = string.Empty;
    }
} 