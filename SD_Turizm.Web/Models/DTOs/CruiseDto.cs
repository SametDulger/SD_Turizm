namespace SD_Turizm.Web.Models.DTOs
{
    public class CruiseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string CariCode { get; set; } = string.Empty;
        public string RoomLocations { get; set; } = string.Empty;
        public string RoomTypes { get; set; } = string.Empty;
        public string ShipName { get; set; } = string.Empty;
        public string Route { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public List<object> Prices { get; set; } = new List<object>();
    }
} 