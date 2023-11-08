using System;
using System.Collections.Generic;

namespace CafeShopFPT.Models
{
    public partial class BillInfo
    {
        public string BillId { get; set; }
        public string FoodId { get; set; }
        public short Quantity { get; set; }
        public int Id { get; set; }

        public virtual Food Food { get; set; }
    }
}
