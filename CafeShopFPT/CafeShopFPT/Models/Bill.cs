using System;
using System.Collections.Generic;

namespace CafeShopFPT.Models
{
    public partial class Bill
    {
        public string BillId { get; set; } = null!;
        public DateTime DateCheckIn { get; set; }
        public DateTime? DateCheckOut { get; set; }
        public string? TableId { get; set; }
        public short Status { get; set; }
        public byte Discount { get; set; }
        public string? AccountId { get; set; }
        public decimal? Total { get; set; }

        public virtual Account? Account { get; set; }
        public virtual TableFood? Table { get; set; }
    }
}
