namespace SD_Turizm.Core.Entities
{
    public class VendorType : BaseEntity
    {
        public string Code { get; set; } = string.Empty; // Airline, Hotel, Cruise, etc.
        public string Name { get; set; } = string.Empty; // Havayolu, Otel, Kruvaziyer, etc.
        public string Description { get; set; } = string.Empty;

        public int DisplayOrder { get; set; }
    }
}
