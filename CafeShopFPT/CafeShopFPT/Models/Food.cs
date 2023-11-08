using System;
using System.Collections.Generic;

namespace CafeShopFPT.Models
{
    public partial class Food
    {
        public Food()
        {
            BillInfos = new HashSet<BillInfo>();
        }

        public string FoodId { get; set; } = null!;
        public string FoodName { get; set; } = null!;
        public string CategoryId { get; set; } = null!;
        public decimal Price { get; set; }
        public string ImgPath { get; set; }

        public virtual Category Category { get; set; } = null!;
        public virtual ICollection<BillInfo> BillInfos { get; set; }
    }
}
