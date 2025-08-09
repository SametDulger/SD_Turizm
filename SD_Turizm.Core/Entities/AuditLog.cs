using System.ComponentModel.DataAnnotations;

namespace SD_Turizm.Core.Entities
{
    public class AuditLog : BaseEntity
    {
        [Required]
        [MaxLength(100)]
        public string TableName { get; set; } = string.Empty;
        
        [Required]
        [MaxLength(50)]
        public string Action { get; set; } = string.Empty; // INSERT, UPDATE, DELETE
        
        [Required]
        public int RecordId { get; set; }
        
        [MaxLength(100)]
        public string? UserId { get; set; }
        
        [MaxLength(100)]
        public string? Username { get; set; }
        
        [MaxLength(45)]
        public string? IpAddress { get; set; }
        
        [MaxLength(500)]
        public string? UserAgent { get; set; }
        
        public string? OldValues { get; set; } // JSON formatında eski değerler
        
        public string? NewValues { get; set; } // JSON formatında yeni değerler
        
        [MaxLength(500)]
        public string? Description { get; set; }
        
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}
