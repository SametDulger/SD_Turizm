namespace SD_Turizm.Core.Entities
{
    public class ProductType : BaseEntity
    {
        public string Code { get; set; } = string.Empty; // Otel, Tur, Transfer, RentACar, Rehber, Uçak, Gemi
        public string Name { get; set; } = string.Empty; // Otel, Tur, Transfer, Rent A Car, Rehber, Uçak, Gemi
        public string Description { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty; // Font Awesome icon class

        public int DisplayOrder { get; set; }
    }
}
