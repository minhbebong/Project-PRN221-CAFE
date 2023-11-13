using CafeShopFPT.DAO.AccountsDao;
using CafeShopFPT.ViewModels.AccountAction;
using System.Windows;

namespace CafeShopFPT.Views {
    /// <summary>
    /// Interaction logic for AccountActionView.xaml
    /// </summary>
    public partial class AccountActionView : Window
    {
        public AccountActionView()
        {
            InitializeComponent();

            this.DataContext = new AccountActionVM();
        }

        public AccountActionView(AccountDTO account) {
            InitializeComponent();
            this.DataContext = new AccountActionVM(account);
        }


    }
}
