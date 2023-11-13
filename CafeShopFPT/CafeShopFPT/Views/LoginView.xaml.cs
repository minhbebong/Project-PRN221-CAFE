using CafeShopFPT.ViewModels.LoginScreen;
using System.Windows;

namespace CafeShopFPT.Views {
    /// <summary>
    /// Interaction logic for LoginView.xaml
    /// </summary>
    public partial class LoginView : Window
    {
        public LoginView()
        {
            InitializeComponent();
            AccountVM accountVM = new AccountVM();
            this.DataContext = accountVM;
        }
    }
}
