namespace SD_Turizm.Web.Models.DTOs
{
    public class HotelPriceDto
    {
        public int Id { get; set; }
        public int HotelId { get; set; }
        public string HotelName { get; set; } = string.Empty;
        public string RoomType { get; set; } = string.Empty;
        public string RoomLocation { get; set; } = string.Empty;
        public string BoardType { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string Currency { get; set; } = string.Empty;
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public DateTime Date { get; set; }
        public decimal AdultPrice { get; set; }
        public decimal ChildPrice { get; set; }
        public decimal InfantPrice { get; set; }
        public DateTime ValidDate { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
    }
} 