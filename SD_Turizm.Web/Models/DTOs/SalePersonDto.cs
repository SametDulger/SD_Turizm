namespace SD_Turizm.Web.Models.DTOs
{
    public class SalePersonDto
    {
        public int Id { get; set; }
        public int SaleId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string PersonType { get; set; } = string.Empty;
        public int Age { get; set; }
        public string Nationality { get; set; } = string.Empty;
        public DateTime? BirthDate { get; set; }
        public string PassportNumber { get; set; } = string.Empty;
    }
} 