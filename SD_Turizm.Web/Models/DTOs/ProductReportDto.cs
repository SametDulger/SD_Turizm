using System.Text.Json.Serialization;

namespace SD_Turizm.Web.Models.DTOs
{
    public class ProductReportDto
    {
        [JsonPropertyName("productName")]
        public string ProductName { get; set; } = string.Empty;
        
        [JsonPropertyName("productType")]
        public string ProductType { get; set; } = string.Empty;
        
        [JsonPropertyName("totalSales")]
        public int TotalSales { get; set; }
        
        [JsonPropertyName("totalRevenue")]
        public decimal TotalRevenue { get; set; }
        
        [JsonPropertyName("totalCost")]
        public decimal TotalCost { get; set; }
        
        [JsonPropertyName("totalProfit")]
        public decimal TotalProfit { get; set; }
        
        [JsonPropertyName("averagePrice")]
        public decimal AveragePrice { get; set; }
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