namespace SD_Turizm.Web.Models.DTOs
{
    public class RentACarPriceDto
    {
        public int Id { get; set; }
        public int RentACarId { get; set; }
        public string RentACarName { get; set; } = string.Empty;
        public string VehicleType { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string Currency { get; set; } = string.Empty;
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public string CarType { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public decimal DailyPrice { get; set; }
        public decimal WeeklyPrice { get; set; }
        public decimal MonthlyPrice { get; set; }
        public decimal AdultPrice { get; set; }
        public decimal ChildPrice { get; set; }
        public decimal InfantPrice { get; set; }
        public string RentACarCompanyName { get; set; } = string.Empty;
        public DateTime ValidDate { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
    }
} 