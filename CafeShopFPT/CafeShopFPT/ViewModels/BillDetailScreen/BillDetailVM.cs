using CafeShopFPT.DAO.BillDao;
using CafeShopFPT.DAO.BillInfoDao;
using ProductCURD01.ViewModel;
using System.Collections.ObjectModel;

namespace CafeShopFPT.ViewModels.BillDetailScreen
{
    public class BillDetailVM : BaseVM
    {
        #region Property
        private BillDTO _currBill;
        public BillDTO CurrBill
        {
            get
            {
                return _currBill;
            }
            set
            {
                _currBill = value; OnPropertyChanged();

                MenuItemList = new ObservableCollection<MenuItemDTO>(BillInfoDao.Instance.GetListFoodOfBill(CurrBill.BillId));
            }
        }


        private ObservableCollection<MenuItemDTO> _menuItemList;
        public ObservableCollection<MenuItemDTO> MenuItemList
        {
            get
            {
                return _menuItemList;
            }
            set
            {
                _menuItemList = value; OnPropertyChanged();
            }
        }


        #endregion

        #region Function

        #endregion
        public BillDetailVM(string billId)
        {
            CurrBill = BillDao.Instance.GetBill(billId);
        }
    }
}
