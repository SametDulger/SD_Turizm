namespace SD_Turizm.Web.Models.DTOs
{
    public class SaleDto
    {
        public int Id { get; set; }
        public string PNRNumber { get; set; } = string.Empty;
        public string CariCode { get; set; } = string.Empty;
        public string CustomerName { get; set; } = string.Empty;
        public DateTime SaleDate { get; set; }
        public string Status { get; set; } = string.Empty;
        public string AgencyCode { get; set; } = string.Empty;
        public string PackageCode { get; set; } = string.Empty;
        public bool IsPackageSale { get; set; }
        public decimal TotalAmount { get; set; }
        public string Currency { get; set; } = string.Empty;
        public string SellerType { get; set; } = string.Empty;
        public string FileCode { get; set; } = string.Empty;
        public decimal SalePrice { get; set; }
        public decimal PurchasePrice { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public decimal TotalAmountTL { get; set; }
        public int? AddressId { get; set; }
        public string SaleNumber { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
} 