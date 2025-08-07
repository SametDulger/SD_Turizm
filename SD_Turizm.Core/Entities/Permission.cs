using System.ComponentModel.DataAnnotations;

namespace SD_Turizm.Core.Entities
{
    public class Permission : BaseEntity
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;
        
        [MaxLength(200)]
        public string? Description { get; set; }
        
        [Required]
        [MaxLength(50)]
        public string Resource { get; set; } = string.Empty;
        
        [Required]
        [MaxLength(20)]
        public string Action { get; set; } = string.Empty;
        
        public virtual ICollection<RolePermission> RolePermissions { get; set; } = new HashSet<RolePermission>();
    }
} 