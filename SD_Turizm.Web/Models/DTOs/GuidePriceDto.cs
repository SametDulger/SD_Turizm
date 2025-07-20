namespace SD_Turizm.Web.Models.DTOs
{
    public class GuidePriceDto
    {
        public int Id { get; set; }
        public int GuideId { get; set; }
        public string Region { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string Currency { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public decimal DailyPrice { get; set; }
        public decimal HalfDayPrice { get; set; }
        public string GuideName { get; set; } = string.Empty;
        public DateTime ValidDate { get; set; }
        public bool IsActive { get; set; }
    }
} 