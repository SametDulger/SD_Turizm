namespace SD_Turizm.Web.Models.DTOs
{
    public class SaleItemDto
    {
        public int Id { get; set; }
        public int SaleId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal Price { get; set; }
        public string ItemType { get; set; } = string.Empty;
        public int ItemId { get; set; }
    }
} 