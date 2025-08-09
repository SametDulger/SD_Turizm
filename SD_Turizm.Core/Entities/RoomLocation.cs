namespace SD_Turizm.Core.Entities
{
    public class RoomLocation : BaseEntity
    {
        public string Code { get; set; } = string.Empty; // DNZ, GNL, PRK
        public string Name { get; set; } = string.Empty; // Deniz Manzaralı, Genel, Park Manzaralı
        public string Description { get; set; } = string.Empty;

        public int DisplayOrder { get; set; }
    }
}
