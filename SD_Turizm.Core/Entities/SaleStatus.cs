namespace SD_Turizm.Core.Entities
{
    public class SaleStatus : BaseEntity
    {
        public string Code { get; set; } = string.Empty; // Pending, Confirmed, Cancelled, Completed
        public string Name { get; set; } = string.Empty; // Beklemede, Onaylandı, İptal Edildi, Tamamlandı
        public string Description { get; set; } = string.Empty;
        public string Color { get; set; } = string.Empty; // CSS color for UI

        public int DisplayOrder { get; set; }
    }
}
