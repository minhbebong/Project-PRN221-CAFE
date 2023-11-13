using CafeShopFPT.LogUlti;
using CafeShopFPT.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CafeShopFPT.DAO.FoodDao
{
    public class FoodDTO : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private string _foodId;
        public string FoodId
        {
            get
            {
                return _foodId;
            }
            set
            {
                _foodId = value; OnPropertyChanged();
            }
        }
        private string _foodName;
        public string FoodName
        {
            get
            {
                return _foodName;
            }
            set
            {
                _foodName = value; OnPropertyChanged();
            }
        }
        private string _categoryId;
        public string CategoryId
        {
            get
            {
                return _categoryId;
            }
            set
            {
                _categoryId = value; OnPropertyChanged();
            }
        }

        private Category _category;
        public Category Category
        {
            get
            {
                return _category;
            }
            set
            {
                _category = value; OnPropertyChanged();
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
        private string? _imgPath;
        public string? ImgPath
        {
            get
            {
                return _imgPath;
            }
            set
            {
                _imgPath = value != null ? FileUlti.GetDestinationPath(value, "Images\\Foods") : string.Empty; OnPropertyChanged();
                OnPropertyChanged();
            }
        }

    }
}
