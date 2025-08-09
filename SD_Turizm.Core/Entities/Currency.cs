namespace SD_Turizm.Core.Entities
{
    public class Currency : BaseEntity
    {
        public string Code { get; set; } = string.Empty; // EUR, USD, TRY
        public string Name { get; set; } = string.Empty; // Euro, Amerikan Doları, Türk Lirası
        public string Symbol { get; set; } = string.Empty; // €, $, ₺
        public string Flag { get; set; } = string.Empty; // 🇪🇺, 🇺🇸, 🇹🇷
        public bool IsActive { get; set; } = true;
        public int DisplayOrder { get; set; }
    }
}
