namespace SD_Turizm.Web.Models.DTOs
{
    public class PackageDto
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal PackagePrice { get; set; }
        public string Currency { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal Price { get; set; }
        public bool IsActive { get; set; }
    }
} 