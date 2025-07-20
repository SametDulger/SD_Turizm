namespace SD_Turizm.Web.Models.DTOs
{
    public class ProductReportDto
    {
        public string ProductName { get; set; } = string.Empty;
        public string ProductType { get; set; } = string.Empty;
        public int TotalSales { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal TotalProfit { get; set; }
        public decimal AveragePrice { get; set; }
        public DateTime LastSaleDate { get; set; }
    }

    public class ProductSummaryDto
    {
        public int TotalProducts { get; set; }
        public int TotalSales { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal TotalProfit { get; set; }
        public decimal AverageProfit { get; set; }
    }
} 