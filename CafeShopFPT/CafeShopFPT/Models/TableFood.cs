using System;
using System.Collections.Generic;

namespace CafeShopFPT.Models
{
    public partial class TableFood
    {
        public TableFood()
        {
            Bills = new HashSet<Bill>();
        }

        public string TableId { get; set; } = null!;
        public string Name { get; set; } = null!;
        public bool Status { get; set; }
        public bool? InUse { get; set; }

        public virtual ICollection<Bill> Bills { get; set; }
    }
}
