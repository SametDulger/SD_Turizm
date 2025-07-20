namespace SD_Turizm.Web.Models.DTOs
{
    public class CustomerReportDto
    {
        public string PnrNumber { get; set; } = string.Empty;
        public string CariCode { get; set; } = string.Empty;
        public string CustomerName { get; set; } = string.Empty;
        public string SellerType { get; set; } = string.Empty;
        public string ProductName { get; set; } = string.Empty;
        public decimal SalePrice { get; set; }
        public DateTime CreatedDate { get; set; }
    }

    public class CustomerSummaryDto
    {
        public int TotalCustomers { get; set; }
        public int TotalSales { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal TotalProfit { get; set; }
        public decimal AverageSaleAmount { get; set; }
    }
} 