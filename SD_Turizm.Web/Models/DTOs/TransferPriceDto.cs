namespace SD_Turizm.Web.Models.DTOs
{
    public class TransferPriceDto
    {
        public int Id { get; set; }
        public int TransferCompanyId { get; set; }
        public string Route { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string Currency { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public int TransferId { get; set; }
        public decimal AdultPrice { get; set; }
        public decimal ChildPrice { get; set; }
        public decimal InfantPrice { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public string TransferCompanyName { get; set; } = string.Empty;
        public DateTime ValidDate { get; set; }
        public bool IsActive { get; set; }
    }
} 