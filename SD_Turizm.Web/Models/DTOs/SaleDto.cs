namespace SD_Turizm.Web.Models.DTOs
{
    public class SaleDto
    {
        public int Id { get; set; }
        public string PnrNumber { get; set; } = string.Empty;
        public int CustomerId { get; set; }
        public int SalePersonId { get; set; }
        public decimal TotalAmount { get; set; }
        public string Currency { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
        public string SaleNumber { get; set; } = string.Empty;
        public DateTime SaleDate { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string CariCode { get; set; } = string.Empty;
        public string AgencyCode { get; set; } = string.Empty;
        public string PackageCode { get; set; } = string.Empty;
    }
} 