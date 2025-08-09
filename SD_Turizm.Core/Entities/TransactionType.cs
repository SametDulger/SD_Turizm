using System;

namespace SD_Turizm.Core.Entities
{
    public class TransactionType : BaseEntity
    {
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int DisplayOrder { get; set; }
    }
}
