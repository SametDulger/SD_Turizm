namespace SD_Turizm.Web.Models.DTOs
{
    public class CariTransactionDto
    {
        public int Id { get; set; }
        public string CariCode { get; set; } = string.Empty;
        public string CustomerName { get; set; } = string.Empty;
        public string TransactionType { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string Currency { get; set; } = string.Empty;
        public DateTime TransactionDate { get; set; }
        public string Description { get; set; } = string.Empty;
    }
} 