namespace SD_Turizm.Web.Models.DTOs
{
    public class TourPriceDto
    {
        public int Id { get; set; }
        public int TourId { get; set; }
        public string TourName { get; set; } = string.Empty;
        public string Season { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string Currency { get; set; } = string.Empty;
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public DateTime ValidDate { get; set; }
        public decimal AdultPrice { get; set; }
        public decimal ChildPrice { get; set; }
        public decimal InfantPrice { get; set; }
        public string TourOperatorName { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
    }
} 