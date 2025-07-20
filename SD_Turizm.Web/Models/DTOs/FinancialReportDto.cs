namespace SD_Turizm.Web.Models.DTOs
{
    public class FinancialReportDto
    {
        public string PnrNumber { get; set; } = string.Empty;
        public string CustomerName { get; set; } = string.Empty;
        public string SellerType { get; set; } = string.Empty;
        public string ProductName { get; set; } = string.Empty;
        public decimal SalePrice { get; set; }
        public decimal PurchasePrice { get; set; }
        public decimal Profit { get; set; }
        public string Currency { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
    }

    public class FinancialSummaryDto
    {
        public int TotalSales { get; set; }
        public decimal TotalSaleAmount { get; set; }
        public decimal TotalPurchaseAmount { get; set; }
        public decimal TotalProfit { get; set; }
        public decimal AverageProfit { get; set; }
        public string Currency { get; set; } = string.Empty;
    }
} 