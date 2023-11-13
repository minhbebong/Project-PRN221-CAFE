using System;
using System.Collections.Generic;
using System.Linq;
using ProductCURD01.ViewModel;
using CafeShopFPT.Views;
using System.Text;
using CafeShopFPT.DAO.AccountsDao;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;

namespace CafeShopFPT.ViewModels.LoginScreen
{
    public class AccountVM : BaseVM
    {

        #region Property
        private Visibility _warningVisiable = Visibility.Hidden;

        public Visibility WarningVisiable
        {
            get
                => _warningVisiable;

            set
            {
                _warningVisiable = value;
                OnPropertyChanged();
            }
        }

        private bool _windowVisiable = true;

        public bool WindowVisiable
        {
            get
                => _windowVisiable;

            set
            {
                _windowVisiable = value;
                OnPropertyChanged();
            }
        }

        private string _userName;
        public string UserName
        {
            get => _userName; set
            {
                _userName = value.Trim();
                OnPropertyChanged();

            }
        }

        private string _password;
        public string Password
        {
            get => _password; set
            {
                _password = value;
                OnPropertyChanged();

            }
        }
        #endregion

        #region Command
        // Command for login execution.
        public ICommand LoginCommand
        {
            get; set;
        }

        // Command for exit current login view.
        public ICommand ExitCommand
        {
            get; set;
        }
        #endregion


        public AccountVM()
        {


            LoginCommand = new RelayCommand<object>((parameter) => {

                if (string.IsNullOrEmpty(UserName) || string.IsNullOrEmpty(Password))
                {

                    return false;

                }
                else
                {

                    return true;
                }

            }, (parameter) => {
                Window thisWindow = parameter as Window;

                var result = AccountDao.Instance.Authorization(UserName, Password);
                if (!result)
                {

                    WarningVisiable = Visibility.Visible;

                }
                else
                {

                    ManagerView managerView = new ManagerView();
                    managerView.Show();
                    thisWindow.Close();

                }

            });

            ExitCommand = new RelayCommand<Window>((parameter) => {
                return true;

            }, (parameter) => {

                if (MessageBox.Show("Do you really want to quit ?", "Exit", MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.OK)
                {
                    parameter?.Close();
                };

            });



        }
    }
}
