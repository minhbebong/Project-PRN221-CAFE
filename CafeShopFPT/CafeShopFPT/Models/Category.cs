using System;
using System.Collections.Generic;

namespace CafeShopFPT.Models
{
    public partial class Category
    {
        public Category()
        {
            Foods = new HashSet<Food>();
        }

        public string CategoryId { get; set; } = null!;
        public string Name { get; set; } = null!;

        public virtual ICollection<Food> Foods { get; set; }
    }
}
