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

        public string FoodId { get; set; }
        public string FoodName { get; set; }
        public string CategoryId { get; set; }
        public decimal Price { get; set; }
        public string ImgPath { get; set; }

        public virtual Category Category { get; set; }
        public virtual ICollection<BillInfo> BillInfos { get; set; }
    }
}
