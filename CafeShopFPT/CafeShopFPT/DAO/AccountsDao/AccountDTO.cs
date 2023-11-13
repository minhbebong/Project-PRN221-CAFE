using CafeShopFPT.LogUlti;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using CafeShopFPT.DAO.RoleDao;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CafeShopFPT.DAO.AccountsDao
{
    public class AccountDTO : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private string _email;
        public string Email
        {
            get
            {
                return _email;
            }
            set
            {
                _email = value; OnPropertyChanged();
            }
        }
        private string _displayName;
        public string DisplayName
        {
            get
            {
                return _displayName;
            }
            set
            {
                _displayName = value; OnPropertyChanged();
            }
        }
        private string _passWord;
        public string PassWord
        {
            get
            {
                return _passWord;
            }
            set
            {
                _passWord = value; OnPropertyChanged();
            }
        }
        private int _type;
        public int Type
        {
            get
            {
                return _type;
            }
            set
            {
                _type = value; OnPropertyChanged();
            }
        }

        private RoleDto _role;
        public RoleDto Role
        {
            get
            {
                return _role;
            }
            set
            {
                _role = value; OnPropertyChanged();
            }
        }

        private string _accountId;
        public string AccountId
        {
            get
            {
                return _accountId;
            }
            set
            {
                _accountId = value; OnPropertyChanged();
            }
        }
        private string? _avatar;
        public string? Avatar
        {
            get
            {
                return _avatar;
            }
            set
            {

                _avatar = value != null ? FileUlti.GetDestinationPath(value, "Images\\Avatars") : string.Empty; OnPropertyChanged();
            }
        }

        private string? _phone;
        public string? Phone
        {
            get
            {
                return _phone;
            }
            set
            {
                _phone = value; OnPropertyChanged();
            }
        }
    }
}
