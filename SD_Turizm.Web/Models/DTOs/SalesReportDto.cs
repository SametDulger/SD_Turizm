using System.Text.Json.Serialization;

namespace SD_Turizm.Web.Models.DTOs
{
    public class SalesReportDto
    {
        [JsonPropertyName("pnrNumber")]
        public string PnrNumber { get; set; } = string.Empty;
        
        [JsonPropertyName("customerName")]
        public string CustomerName { get; set; } = string.Empty;
        
        [JsonPropertyName("sellerType")]
        public string SellerType { get; set; } = string.Empty;
        
        [JsonPropertyName("productName")]
        public string ProductName { get; set; } = string.Empty;
        
        [JsonPropertyName("salePrice")]
        public decimal SalePrice { get; set; }
        
        [JsonPropertyName("purchasePrice")]
        public decimal PurchasePrice { get; set; }
        
        [JsonPropertyName("createdDate")]
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