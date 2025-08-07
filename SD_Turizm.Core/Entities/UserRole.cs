namespace SD_Turizm.Core.Entities
{
    public class UserRole : BaseEntity
    {
        public int UserId { get; set; }
        public virtual User User { get; set; } = null!;
        
        public int RoleId { get; set; }
        public virtual Role Role { get; set; } = null!;
    }
} 