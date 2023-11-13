using CafeShopFPT.DAO.AccountsDao;
using System.Windows.Controls;
using System.Windows;
using CafeShopFPT.ViewModels.AccountAction;

namespace CafeShopFPT.Views
{
    /// <summary>
    /// Interaction logic for AccountActionView.xaml
    /// </summary>
    public partial class AccountActionView : Page
    {
        public AccountActionView()
        {
            InitializeComponent();

            this.DataContext = new AccountActionVM();
        }

        public AccountActionView(AccountDTO account)
        {
            InitializeComponent();
            this.DataContext = new AccountActionVM(account);
        }
    }
}
