using CafeShopFPT.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CafeShopFPT.DAO.BillDao
{
    public class BillDTO : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private string _billId;
        public string BillId
        {
            get
            {
                return _billId;
            }
            set
            {
                _billId = value; OnPropertyChanged();
            }
        }
        private DateTime _dateCheckIn;
        public DateTime DateCheckIn
        {
            get
            {
                return _dateCheckIn;
            }
            set
            {
                _dateCheckIn = value; OnPropertyChanged();
            }
        }
        private DateTime? _dateCheckOut;
        public DateTime? DateCheckOut
        {
            get
            {
                return _dateCheckOut;
            }
            set
            {
                _dateCheckOut = value; OnPropertyChanged();
            }
        }
        private string _tableId;
        public string TableId
        {
            get
            {
                return _tableId;
            }
            set
            {
                _tableId = value; OnPropertyChanged();
            }
        }

        private TableFood _table;
        public TableFood Table
        {
            get
            {
                return _table;
            }
            set
            {
                _table = value; OnPropertyChanged();
            }
        }
        private short _status;
        public short Status
        {
            get
            {
                return _status;
            }
            set
            {
                _status = value; OnPropertyChanged();
            }
        }
        private byte _discount;
        public byte Discount
        {
            get
            {
                return _discount;
            }
            set
            {
                _discount = value; OnPropertyChanged();
            }
        }
        private string? _accountId;
        public string? AccountId
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

        private Account? _account;
        public Account? Account
        {
            get
            {
                return _account;
            }
            set
            {
                _account = value; OnPropertyChanged();
            }
        }
        private decimal? _total;
        public decimal? Total
        {
            get
            {
                return _total;
            }
            set
            {
                _total = value; OnPropertyChanged();
            }
        }

    }
}
