namespace SD_Turizm.Web.Models.DTOs
{
    public class TourDto
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string VehicleType { get; set; } = string.Empty;
        public string Region { get; set; } = string.Empty;
        public int TourOperatorId { get; set; }
        public string Description { get; set; } = string.Empty;
        public string Duration { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public string TourOperatorName { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
} 