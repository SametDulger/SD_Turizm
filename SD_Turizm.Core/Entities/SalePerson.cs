using System;
using System.ComponentModel.DataAnnotations;

namespace SD_Turizm.Core.Entities
{
    public class SalePerson : BaseEntity
    {
        public int SaleId { get; set; }
        public virtual Sale Sale { get; set; } = null!;
        
        [Required]
        [MaxLength(100)]
        public string FirstName { get; set; } = string.Empty;
        
        [Required]
        [MaxLength(100)]
        public string LastName { get; set; } = string.Empty;
        
        [Required]
        [MaxLength(10)]
        public string PersonType { get; set; } = string.Empty; // PAX, CHD, INF
        
        public int Age { get; set; }
        
        [MaxLength(50)]
        public string? Nationality { get; set; }
        
        // Web view'lar için ek property'ler
        public DateTime? BirthDate { get; set; }
        
        [MaxLength(50)]
        public string? PassportNumber { get; set; }
    }
} 