namespace SD_Turizm.Web.Models.DTOs
{
    public class AirlineDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public string AircraftTypes { get; set; } = string.Empty;
        public string FlightRegions { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }
} 