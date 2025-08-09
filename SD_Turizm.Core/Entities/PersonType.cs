namespace SD_Turizm.Core.Entities
{
    public class PersonType : BaseEntity
    {
        public string Code { get; set; } = string.Empty; // PAX, CHD, INF
        public string Name { get; set; } = string.Empty; // Yolcu, Çocuk, Bebek
        public string Description { get; set; } = string.Empty;
        public int MinAge { get; set; } // Minimum yaş
        public int MaxAge { get; set; } // Maximum yaş

        public int DisplayOrder { get; set; }
    }
}
