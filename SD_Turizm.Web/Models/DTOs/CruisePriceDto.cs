namespace SD_Turizm.Web.Models.DTOs
{
    public class CruisePriceDto
    {
        public int Id { get; set; }
        public int CruiseId { get; set; }
        public string CruiseName { get; set; } = string.Empty;
        public string CabinType { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string Currency { get; set; } = string.Empty;
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public DateTime Date { get; set; }
        public decimal AdultPrice { get; set; }
        public decimal ChildPrice { get; set; }
        public decimal InfantPrice { get; set; }
        public string ShipName { get; set; } = string.Empty;
        public DateTime ValidDate { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
    }
} 