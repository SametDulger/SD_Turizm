namespace SD_Turizm.Web.Models.DTOs
{
    public class TourOperatorDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string CariCode { get; set; } = string.Empty;
        public string Notes { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public List<object> Tours { get; set; } = new List<object>();
        public List<object> TourPrices { get; set; } = new List<object>();
    }
} 