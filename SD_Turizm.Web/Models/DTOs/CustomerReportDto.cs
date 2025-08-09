using System.Text.Json.Serialization;

namespace SD_Turizm.Web.Models.DTOs
{
    public class CustomerReportDto
    {
        [JsonPropertyName("cariCode")]
        public string CariCode { get; set; } = string.Empty;
        
        [JsonPropertyName("customerName")]
        public string CustomerName { get; set; } = string.Empty;
        
        [JsonPropertyName("totalOrders")]
        public int TotalOrders { get; set; }
        
        [JsonPropertyName("totalPurchases")]
        public decimal TotalPurchases { get; set; }
        
        [JsonPropertyName("totalProfit")]
        public decimal TotalProfit { get; set; }
        
        [JsonPropertyName("averageOrderValue")]
        public decimal AverageOrderValue { get; set; }
        
        [JsonPropertyName("firstPurchaseDate")]
        public DateTime FirstPurchaseDate { get; set; }
        
        [JsonPropertyName("lastPurchaseDate")]
        public DateTime LastPurchaseDate { get; set; }
        
        [JsonPropertyName("productName")]
        public string ProductName { get; set; } = string.Empty;
        
        [JsonPropertyName("sellerType")]
        public string SellerType { get; set; } = string.Empty;
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