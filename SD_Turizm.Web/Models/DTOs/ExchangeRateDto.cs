namespace SD_Turizm.Web.Models.DTOs
{
    public class ExchangeRateDto
    {
        public int Id { get; set; }
        public string FromCurrency { get; set; } = string.Empty;
        public string ToCurrency { get; set; } = string.Empty;
        public decimal Rate { get; set; }
        public DateTime Date { get; set; }
        public string Source { get; set; } = string.Empty;
        public string SourceCurrency { get; set; } = string.Empty;
        public string TargetCurrency { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
    }
} 