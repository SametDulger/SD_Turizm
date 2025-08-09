namespace SD_Turizm.Core.Entities
{
    public class BoardType : BaseEntity
    {
        public string Code { get; set; } = string.Empty; // OB, BB, HB, FB, AI, UAI
        public string Name { get; set; } = string.Empty; // Oda Kahvaltı, Yarım Pansiyon, etc.
        public string Description { get; set; } = string.Empty;

        public int DisplayOrder { get; set; }
    }
}
