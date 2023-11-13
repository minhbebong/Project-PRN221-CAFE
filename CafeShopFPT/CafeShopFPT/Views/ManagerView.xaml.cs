using CafeShopFPT.ViewModels;
using System.Windows;

namespace CafeShopFPT.Views {
    /// <summary>
    /// Interaction logic for ManagerView.xaml
    /// </summary>
    public partial class ManagerView :Window {
        public ManagerView() {
            InitializeComponent();
            this.DataContext = new ManagerVM();
        }

    }
}
