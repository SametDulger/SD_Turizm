namespace SD_Turizm.Core.Entities
{
    public class RoomType : BaseEntity
    {
        public string Code { get; set; } = string.Empty; // SGL, DBL, TRP, etc.
        public string Name { get; set; } = string.Empty; // Tek Kişilik, Çift Kişilik, etc.
        public string Description { get; set; } = string.Empty;
        public int Capacity { get; set; } // Kaç kişilik

        public int DisplayOrder { get; set; }
    }
}
