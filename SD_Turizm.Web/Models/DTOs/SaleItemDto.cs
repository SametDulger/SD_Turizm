namespace SD_Turizm.Web.Models.DTOs
{
    public class SaleItemDto
    {
        public int Id { get; set; }
        public int SaleId { get; set; }
        public DateTime Date { get; set; }
        public string ServiceType { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int? VendorId { get; set; }
        public string? VendorType { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
        public string Currency { get; set; } = string.Empty;
        public string? ItemType { get; set; }
        public int ItemId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public DateTime CreatedAt { get; set; }
    }
} 