using System.ComponentModel.DataAnnotations;

namespace SD_Turizm.Core.Entities
{
    public class Role : BaseEntity
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;
        
        [MaxLength(200)]
        public string? Description { get; set; }
        
        public virtual ICollection<UserRole> UserRoles { get; set; } = new HashSet<UserRole>();
        
        public virtual ICollection<RolePermission> RolePermissions { get; set; } = new HashSet<RolePermission>();
    }
} 