using System;
using System.Collections.Generic;

namespace CafeShopFPT.Models
{
    public partial class Account
    {
        public Account()
        {
            Bills = new HashSet<Bill>();
        }

        public string UserName { get; set; }
        public string DisplayName { get; set; }
        public string PassWord { get; set; }
        public int Type { get; set; }
        public string AccountId { get; set; }
        public string Avatar { get; set; }
        public string Phone { get; set; }

        public virtual Role TypeNavigation { get; set; }
        public virtual ICollection<Bill> Bills { get; set; }
    }
}
