namespace SD_Turizm.Core.DTOs
{
    public class SalesReportDto
    {
        public int Id { get; set; }
        public string PNR { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public string Agency { get; set; } = string.Empty;
        public string Cari { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string Status { get; set; } = string.Empty;
        public string ProductType { get; set; } = string.Empty;
        public string ProductName { get; set; } = string.Empty;
    }

    public class FinancialReportDto
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Period { get; set; } = string.Empty;
        public string Currency { get; set; } = string.Empty;
        public decimal TotalRevenue { get; set; }
        public decimal TotalCost { get; set; }
        public decimal GrossProfit { get; set; }
        public decimal NetProfit { get; set; }
        public string Category { get; set; } = string.Empty;
    }

    public class CustomerReportDto
    {
        public int Id { get; set; }
        public string CustomerCode { get; set; } = string.Empty;
        public string CustomerName { get; set; } = string.Empty;
        public string CustomerType { get; set; } = string.Empty;
        public int TotalSales { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime LastPurchaseDate { get; set; }
        public string Status { get; set; } = string.Empty;
    }

    public class ProductReportDto
    {
        public int Id { get; set; }
        public string ProductType { get; set; } = string.Empty;
        public string ProductName { get; set; } = string.Empty;
        public string ProductCode { get; set; } = string.Empty;
        public int TotalSales { get; set; }
        public decimal TotalRevenue { get; set; }
        public decimal AveragePrice { get; set; }
        public DateTime LastSaleDate { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}
