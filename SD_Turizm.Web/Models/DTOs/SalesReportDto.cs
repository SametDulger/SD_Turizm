namespace SD_Turizm.Web.Models.DTOs
{
    public class SalesReportDto
    {
        public string PnrNumber { get; set; } = string.Empty;
        public string CustomerName { get; set; } = string.Empty;
        public string SellerType { get; set; } = string.Empty;
        public string ProductName { get; set; } = string.Empty;
        public decimal SalePrice { get; set; }
        public decimal PurchasePrice { get; set; }
        public DateTime CreatedDate { get; set; }
    }

    public class SalesSummaryDto
    {
        public int TotalSales { get; set; }
        public decimal TotalSaleAmount { get; set; }
        public decimal TotalPurchaseAmount { get; set; }
        public decimal TotalProfit { get; set; }
    }
} 