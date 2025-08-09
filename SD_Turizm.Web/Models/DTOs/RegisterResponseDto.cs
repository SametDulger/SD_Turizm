namespace SD_Turizm.Web.Models.DTOs
{
    public class RegisterResponseDto
    {
        public string Token { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public DateTime ExpiresAt { get; set; }
    }
}
