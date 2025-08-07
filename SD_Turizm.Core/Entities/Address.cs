using System.ComponentModel.DataAnnotations;

namespace SD_Turizm.Core.Entities
{
    public class Address : BaseEntity
    {
        [Required]
        [MaxLength(200)]
        public string Street { get; set; } = string.Empty;
        
        [Required]
        [MaxLength(100)]
        public string City { get; set; } = string.Empty;
        
        [Required]
        [MaxLength(100)]
        public string Country { get; set; } = string.Empty;
        
        [MaxLength(20)]
        public string? PostalCode { get; set; }
        
        [MaxLength(500)]
        public string? FullAddress { get; set; }
    }
} 