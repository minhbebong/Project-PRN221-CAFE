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

        public string UserName { get; set; } = null!;
        public string DisplayName { get; set; } = null!;
        public string PassWord { get; set; } = null!;
        public int Type { get; set; }
        public string AccountId { get; set; } = null!;
        public string Avatar { get; set; }
        public string Phone { get; set; }

        public virtual Role TypeNavigation { get; set; } = null!;
        public virtual ICollection<Bill> Bills { get; set; }
    }
}
