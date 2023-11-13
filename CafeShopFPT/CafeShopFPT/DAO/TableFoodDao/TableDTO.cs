using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CafeShopFPT.DAO.TableFoodDao {
    public class TableDTO :INotifyPropertyChanged {
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null) {
            PropertyChanged?.Invoke(this,new PropertyChangedEventArgs(propertyName));
        }

        private string _tableId;
        public string TableId {
            get { return _tableId;  }
            set {
                _tableId = value; OnPropertyChanged();
            } }

        private string _name;
        public string Name {
            get {
                return _name;
            }
            set {
                _name = value; OnPropertyChanged();
            }
        }

        private bool _status;
        public bool Status {
            get {
                return _status;
            }
            set {
                _status = value; OnPropertyChanged();
            }
        }

        private bool? _inUse;
        public bool? InUse {
            get {
                return _inUse;
            }
            set {
                _inUse = value; OnPropertyChanged();
            }
        }
    }
}
