using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CafeShopFPT.DAO.CategoryDao {
    public class CategoryDTO :INotifyPropertyChanged {
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null) {
            PropertyChanged?.Invoke(this,new PropertyChangedEventArgs(propertyName));
        }

        private string _categoryId;
        public string CategoryId {
            get {
                return _categoryId;
            }
            set {
                _categoryId = value; OnPropertyChanged();
            }
        }

        private string _name;
        public string Name {
            get {
                return _name;
            }
            set {
                _name = value; OnPropertyChanged();
            }
        }

    }
}
