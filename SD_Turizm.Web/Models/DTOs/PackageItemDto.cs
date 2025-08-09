using System.ComponentModel.DataAnnotations;

namespace SD_Turizm.Web.Models.DTOs
{
    public class PackageItemDto
    {
        public int Id { get; set; }
        public int PackageId { get; set; }
        public string PackageName { get; set; } = string.Empty;
        public string ServiceType { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string Currency { get; set; } = string.Empty;
        public string ItemType { get; set; } = string.Empty;
        public int ItemId { get; set; }
        public int Order { get; set; }
        public string Notes { get; set; } = string.Empty;
        public int Sequence { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
    }
} 