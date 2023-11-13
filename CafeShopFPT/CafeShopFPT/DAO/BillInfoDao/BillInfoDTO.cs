using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CafeShopFPT.DAO.BillInfoDao {
    public class BillInfoDTO :INotifyPropertyChanged {
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null) {
            PropertyChanged?.Invoke(this,new PropertyChangedEventArgs(propertyName));
        }

                private string _billId ;
        public string BillId {
            get {
                return _billId;
            }
            set {
                _billId = value; OnPropertyChanged();
            }
        }        private string _foodId ;
        public string FoodId {
            get {
                return _foodId;
            }
            set {
                _foodId = value; OnPropertyChanged();
            }
        }        private short _quantity ;
        public short Quantity {
            get {
                return _quantity;
            }
            set {
                _quantity = value; OnPropertyChanged();
            }
        }

    }
}
