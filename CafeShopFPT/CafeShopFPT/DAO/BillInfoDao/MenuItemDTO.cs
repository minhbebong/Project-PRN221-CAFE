using CafeShopFPT.DAO.FoodDao;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CafeShopFPT.DAO.BillInfoDao
{
    public class MenuItemDTO : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private FoodDTO _food;
        public FoodDTO Food
        {
            get
            {
                return _food;
            }
            set
            {
                _food = value; OnPropertyChanged();
            }
        }

        private short _quantity;
        public short Quantity
        {
            get
            {
                return _quantity;
            }
            set
            {
                _quantity = value; OnPropertyChanged();
            }
        }

        private decimal _price;
        public decimal Price
        {
            get
            {
                return _price;
            }
            set
            {
                _price = value; OnPropertyChanged();
            }
        }

        private decimal _total;
        public decimal Total
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
