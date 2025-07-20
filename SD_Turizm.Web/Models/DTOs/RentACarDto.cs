namespace SD_Turizm.Web.Models.DTOs
{
    public class RentACarDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string CarTypes { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public string CompanyName { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }
} 