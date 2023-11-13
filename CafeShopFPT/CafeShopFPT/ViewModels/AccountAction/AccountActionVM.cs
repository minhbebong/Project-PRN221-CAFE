using ProductCURD01.ViewModel;
using System;
using System.Windows;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Microsoft.Win32;
using DocumentFormat.OpenXml;
using System.IO;
using CafeShopFPT.DAO.AccountsDao;
using CafeShopFPT.DAO.RoleDao;
using CafeShopFPT.LogUlti;

namespace CafeShopFPT.ViewModels.AccountAction
{
    public class AccountActionVM : BaseVM
    {

        #region Property

        private string _oldPassword;
        public string OldPassword
        {
            get => _oldPassword;
            set
            {

                _oldPassword = value;
                OnPropertyChanged();

                if (string.IsNullOrEmpty(value) && IsCheckChangePassword)
                {
                    throw new Exception("Field is required!");
                }
            }
        }

        private string _newPassword;
        public string NewPassword
        {
            get => _newPassword;
            set
            {

                _newPassword = value;
                OnPropertyChanged();

                if (string.IsNullOrEmpty(value) && IsCheckChangePassword)
                {
                    throw new Exception("Field is required!");
                }
            }
        }

        private string _confirmPassword;
        public string ConfirmPassword
        {
            get => _confirmPassword;
            set
            {
                _confirmPassword = value;
                OnPropertyChanged();

                if (string.IsNullOrEmpty(value) && IsCheckChangePassword)
                {
                    throw new Exception("Field is required!");
                }

            }
        }

        private AccountDTO _selectAccount = new AccountDTO();
        public AccountDTO SelectAccount
        {
            get => _selectAccount;
            set
            {
                _selectAccount = value;
                OnPropertyChanged();

                SelectRole = SelectAccount.Type;
            }
        }

        private int _selectRole;
        public int SelectRole
        {
            get => _selectRole;
            set
            {
                _selectRole = value;
                OnPropertyChanged();

                SelectAccount.Type = SelectRole;
            }
        }

        private bool _isCheckChangePassword;
        public bool IsCheckChangePassword
        {
            get
                => _isCheckChangePassword;

            set
            {
                _isCheckChangePassword = value;
            }
        }



        private bool _isChangePassword = true;
        public bool IsChangePassword
        {
            get
                => _isChangePassword;

            set
            {
                _isChangePassword = value;
            }
        }

        private bool _createPasswordVisibility = true;

        public bool CreatePasswordVisibility
        {
            get
                => _createPasswordVisibility;

            set
            {
                _createPasswordVisibility = value;
                OnPropertyChanged();
            }
        }

        private bool _changePasswordVisibility = true;

        public bool UpdatePasswordVisibility
        {
            get
                => _changePasswordVisibility;

            set
            {
                _changePasswordVisibility = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<RoleDto> _roleList;
        public ObservableCollection<RoleDto> RoleList
        {
            get
            {
                return _roleList;
            }
            set
            {
                _roleList = value; OnPropertyChanged();
            }
        }
        #endregion

        #region Function
        private void ChangeVisibility()
        {
            if (IsChangePassword)
            {
                CreatePasswordVisibility = false;
                UpdatePasswordVisibility = true;
            }
            else
            {
                CreatePasswordVisibility = true;
                UpdatePasswordVisibility = false;
            }
        }

        private void Setup(bool isChangePassword)
        {
            IsChangePassword = isChangePassword;
            ChangeVisibility();
            LoadRoleData();
        }


        private void LoadRoleData()
        {
            RoleList = new ObservableCollection<RoleDto>(RoleDao.Instance.LoadAllRoles());
        }


        private void UploadImage()
        {
            // Create OpenFileDialog
            OpenFileDialog dlg = new OpenFileDialog();

            // Set filter for file extension and default file extension
            dlg.DefaultExt = ".png";
            dlg.Filter = "All files (*.*)|*.*|Image files (*.png;*.jpeg)|*.png;*.jpeg";

            // Display OpenFileDialog by calling ShowDialog method
            bool? result = dlg.ShowDialog();

            if (result == true)
            {
                // Open document
                SelectAccount.Avatar = dlg.FileName;

                if (result == true)
                {
                    // Open document
                    string filepath = dlg.FileName; // Stores Original Path in Textbox
                    string name = System.IO.Path.GetFileName(filepath);
                    string destinationPath = FileUlti.GetDestinationPath(name, "Images\\Avatars");

                    File.Copy(filepath, destinationPath, true);

                    SelectAccount.Avatar = name;
                }

            }
        }

        private bool CheckRequiredData()
        {
            if (string.IsNullOrEmpty(SelectAccount.Email) || string.IsNullOrEmpty(SelectAccount.DisplayName))
            {
                return false;
            }

            if (IsChangePassword)
            {

                if (IsCheckChangePassword)
                {
                    if (string.IsNullOrEmpty(OldPassword) || string.IsNullOrEmpty(NewPassword) || string.IsNullOrEmpty(ConfirmPassword))
                    {
                        return false;
                    }
                }


            }
            else
            {
                if (string.IsNullOrEmpty(NewPassword) || string.IsNullOrEmpty(ConfirmPassword))
                {
                    return false;
                }
            }

            return true;
        }
        #endregion

        #region Command
        public ICommand ResetCommand
        {
            get; set;
        }

        public ICommand SaveChangesCommand
        {
            get; set;
        }

        public ICommand UploadAvatarCommand
        {
            get; set;
        }

        #endregion

        public AccountActionVM()
        {
            Setup(false);


            ResetCommand = new RelayCommand<object>((p) =>
            {

                return true;


            }, (p) =>
            {

                SelectAccount = new AccountDTO();


            });

            SaveChangesCommand = new RelayCommand<object>((p) =>
            {

                return CheckRequiredData();

            }, (p) =>
            {
                Window thisWindow = p as Window;

                if (AccountDao.Instance.IsAccountExist(SelectAccount.Email))
                {
                    MessageBox.Show("Email exist!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (!NewPassword.Equals(ConfirmPassword))
                {
                    MessageBox.Show("New password not match the confirm password!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                SelectAccount.PassWord = BCrypt.Net.BCrypt.HashPassword(ConfirmPassword);
                SelectAccount.AccountId = AccountDao.Instance.GetAccountIdMax();
                if (AccountDao.Instance.AddAccount(SelectAccount))
                {

                    MessageBox.Show("Add account successfully!", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                    thisWindow.Close();
                }
                else
                {

                    MessageBox.Show("Add account fail!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }

            });




            UploadAvatarCommand = new RelayCommand<object>((p) =>
            {
                return true;

            }, (p) =>
            {
                UploadImage();
            });
        }

        public AccountActionVM(AccountDTO selectedAccount)
        {

            Setup(true);

            SelectAccount = selectedAccount;
            var oldAccount = selectedAccount;

            ResetCommand = new RelayCommand<object>((p) =>
            {

                return false;

            }, (p) =>
            {

                SelectAccount = oldAccount;



            });

            SaveChangesCommand = new RelayCommand<object>((p) =>
            {

                return CheckRequiredData();

            }, (p) =>
            {
                Window thisWindow = p as Window;


                if (IsCheckChangePassword)
                {

                    if (!NewPassword.Equals(ConfirmPassword))
                    {
                        MessageBox.Show("New password not match the confirm password!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    bool verified = BCrypt.Net.BCrypt.Verify(OldPassword, SelectAccount.PassWord);
                    if (!verified)
                    {
                        MessageBox.Show("Old password not match the current password!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    SelectAccount.PassWord = BCrypt.Net.BCrypt.HashPassword(ConfirmPassword);
                }


                if (AccountDao.Instance.UpdateAccount(SelectAccount))
                {

                    MessageBox.Show("Update account successfully!", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                    thisWindow.Close();
                }
                else
                {

                    MessageBox.Show("Update account fail!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }


            });

            UploadAvatarCommand = new RelayCommand<object>((p) =>
            {
                return true;

            }, (p) =>
            {
                UploadImage();

            });

        }
    }
}
