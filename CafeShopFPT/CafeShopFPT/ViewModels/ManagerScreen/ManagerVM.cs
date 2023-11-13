
using CafeShopFPT.DAO.AccountsDao;
using CafeShopFPT.DAO.BillDao;
using CafeShopFPT.DAO.BillInfoDao;
using CafeShopFPT.DAO.CategoryDao;
using CafeShopFPT.DAO.FoodDao;
using CafeShopFPT.DAO.TableFoodDao;
using CafeShopFPT.Views;
using ProductCURD01.ViewModel;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CafeShopFPT.ViewModels {
    public class ManagerVM :BaseVM {

        #region Property

        private bool _windowVisiable = true;

        public bool WindowVisiable {
            get
                => _windowVisiable;

            set {
                _windowVisiable = value;
                OnPropertyChanged();
            }
        }

        private bool _isAdmin = false;
       public bool IsAdmin {
            get {
                return _isAdmin;
            }
            set {
                _isAdmin = value; OnPropertyChanged();

            }
        }

        private ObservableCollection<MenuItemDTO> _menuItemList;
        public ObservableCollection<MenuItemDTO> MenuItemList {
            get {
                return _menuItemList;
            }
            set {
                _menuItemList = value; OnPropertyChanged();
            }
        }

        private MenuItemDTO _selectedMenuItem;
        public MenuItemDTO SelectedMenuItem {
            get {
                return _selectedMenuItem;
            }
            set {
                _selectedMenuItem = value; OnPropertyChanged();
                if (SelectedMenuItem != null) {
                    SelectedCategory = CategoriesList.First(x => x.CategoryId.Equals(SelectedMenuItem.Food.CategoryId));
                    SelectedFood = FoodList.First(x => x.FoodId.Equals(SelectedMenuItem.Food.FoodId));
                }
            }
        }

        private ObservableCollection<TableDTO> _tablesList;
        public ObservableCollection<TableDTO> TablesList {
            get {
                return _tablesList;
            }
            set {
                _tablesList = value; OnPropertyChanged();
            }
        }

        private TableDTO _selectedTable;
        public TableDTO SelectedTable {
            get {
                return _selectedTable;
            }
            set {
                _selectedTable = value; OnPropertyChanged();
                if (SelectedTable != null) {
                    BillId = BillDao.Instance.GetUncheckBillIDByTableID(SelectedTable.TableId);

                }

            }
        }


        private ObservableCollection<BillDTO> _takeawayList;
        public ObservableCollection<BillDTO> TakeawayList {
            get {
                return _takeawayList;
            }
            set {
                _takeawayList = value; OnPropertyChanged();
            }
        }

        private BillDTO _selectedTakeaway;
        public BillDTO SelectedTakeaway {
            get {
                return _selectedTakeaway;
            }
            set {
                _selectedTakeaway = value; OnPropertyChanged();
                if (SelectedTakeaway != null) {
                    BillId = SelectedTakeaway.BillId;

                }

            }
        }


        private bool _isTakeaway;
        public bool IsTakeaway {
            get {
                return _isTakeaway;
            }
            set {
                _isTakeaway = value; OnPropertyChanged();
                SelectedTable = null;
                SelectedTakeaway = null;

            }
        }

        private ObservableCollection<CategoryDTO> _categoriesList;
        public ObservableCollection<CategoryDTO> CategoriesList {
            get {
                return _categoriesList;
            }
            set {
                _categoriesList = value; OnPropertyChanged();
            }
        }

        private CategoryDTO _selectedCategory;
        public CategoryDTO SelectedCategory {
            get {
                return _selectedCategory;
            }
            set {
                _selectedCategory = value; OnPropertyChanged();
                FoodList = new ObservableCollection<FoodDTO>(FoodDao.Instance.LoadAllFoodByCategoryId(SelectedCategory.CategoryId));
                if (SelectedFood == null) {
                    SelectedFood = FoodList.First();
                }
            }
        }

        private FoodDTO _selectedFood;
        public FoodDTO SelectedFood {
            get {
                return _selectedFood;
            }
            set {
                _selectedFood = value; OnPropertyChanged();
            }
        }

        private ObservableCollection<FoodDTO> _foodList;
        public ObservableCollection<FoodDTO> FoodList {
            get {
                return _foodList;
            }
            set {
                _foodList = value; OnPropertyChanged();
            }
        }

        private int _foodQuantity = 1;
        public int FoodQuantity {
            get {
                return _foodQuantity;
            }
            set {
                _foodQuantity = value; OnPropertyChanged();
            }
        }

        private int _discount = 0;
        public int Discount {
            get {
                return _discount;
            }
            set {
                _discount = value; OnPropertyChanged();
                CalculateDiscount();
            }
        }
        private float _total = 0;
        public float Total {
            get {
                return _total;
            }
            set {
                _total = value; OnPropertyChanged();
            }
        }

        private string? _billId;
        public string? BillId {
            get {
                return _billId;
            }
            set {
                _billId = value; OnPropertyChanged();

            }
        }


        #endregion

        #region Function
        private void LoadTableData() {
            TablesList = new ObservableCollection<TableDTO>(TablesFoodDao.Instance.LoadAllTables(false));
            if (SelectedTable != null) {
                SelectedTable = TablesList.First(x => x.TableId.Equals(SelectedTable.TableId));

            }
        }

        private void LoadTakeawayData() {
            TakeawayList = new ObservableCollection<BillDTO>(BillDao.Instance.LoadAllTakeAway());

        }

        private void LoadCategoriesData() {
            CategoriesList = new ObservableCollection<CategoryDTO>(CategoryDao.Instance.LoadAllCategories());
            SelectedCategory = CategoriesList.First();
        }

        private void LoadMenuTakeawayData(string billId) {
            MenuItemList = new ObservableCollection<MenuItemDTO>(BillInfoDao.Instance.GetListFoodTakeaway(billId));
            CalculateDiscount();


        }

        private void LoadMenuData(string tableId) {
            MenuItemList = new ObservableCollection<MenuItemDTO>(BillInfoDao.Instance.GetListFoodOrder(tableId));
            CalculateDiscount();


        }

        private void CalculateDiscount() {
            var tempTotal = float.Parse(MenuItemList.Sum(x => x.Total).ToString());
            var discount = (tempTotal * Discount) / 100;

            Total = tempTotal - discount;
        }



        #endregion


        #region Command
        public ICommand TableSelectCommand {
            get; set;
        }

        public ICommand TakeawaySelectCommand {
            get; set;
        }

        public ICommand AddItemCommand {
            get; set;
        }

        public ICommand SubItemCommand {
            get; set;
        }
        public ICommand ClearMenuCommand {
            get; set;
        }

        public ICommand CheckOutCommand {
            get; set;
        }

        public ICommand AddTakeawayCommand {
            get; set;
        }

        public ICommand RemoveTakeawayCommand {
            get; set;
        }

        public ICommand OpenAdminOptionsCommand {
            get; set;
        }

        public ICommand OpenProfileOptionsCommand {
            get; set;
        }
        public ICommand OpenViewBillsOptionsCommand {
            get; set;
        }

        #endregion



        public ManagerVM() {
            LoadTableData();
            LoadTakeawayData();
            LoadCategoriesData();
            if (AccountDao.Instance.CurrrentUser.Type.Equals(1)) {
                IsAdmin = true;
            }

            TableSelectCommand = new RelayCommand<object>((p) => {

                return true;

            },(p) => {

                SelectedTable = ((p as Button).DataContext as TableDTO);
                if (SelectedTable != null) {
                    LoadMenuData(SelectedTable.TableId);
                }


            });

            TakeawaySelectCommand = new RelayCommand<object>((p) => {

                return true;

            },(p) => {

                SelectedTakeaway = ((p as Button).DataContext as BillDTO);
                if (SelectedTakeaway != null) {
                    LoadMenuTakeawayData(SelectedTakeaway.BillId);
                }


            });

            AddItemCommand = new RelayCommand<object>((p) => {

                return true;

            },(p) => {


                if (IsTakeaway) {
                    if (SelectedTakeaway == null) {
                        MessageBox.Show("Choose a bill first !","Notification",MessageBoxButton.OK,MessageBoxImage.Information);

                    } else {

                        string foodId = SelectedFood != null ? SelectedFood.FoodId : "0";


                        if (BillId != null) {

                            BillInfoDao.Instance.InsertBillInfo(BillId,foodId,Convert.ToInt16(FoodQuantity));
                        }

                        LoadMenuTakeawayData(BillId);
                        LoadTakeawayData();
                    }
                } else {
                    if (SelectedTable == null) {
                        MessageBox.Show("Choose a table first !","Notification",MessageBoxButton.OK,MessageBoxImage.Information);

                    } else {

                        string foodId = SelectedFood != null ? SelectedFood.FoodId : "0";


                        if (BillId == null) {
                            string maxBillId = BillDao.Instance.GetBillIdMax();
                            bool insertBill = BillDao.Instance.InsertBill(maxBillId,SelectedTable.TableId,AccountDao.Instance.CurrrentUser.AccountId);
                            if (insertBill) {


                                BillInfoDao.Instance.InsertBillInfo(maxBillId,foodId,Convert.ToInt16(FoodQuantity));
                            }
                        } else {
                            BillInfoDao.Instance.InsertBillInfo(BillId,foodId,Convert.ToInt16(FoodQuantity));
                        }

                        LoadMenuData(SelectedTable.TableId);
                        LoadTableData();
                    }
                }


            });

            SubItemCommand = new RelayCommand<object>((p) => {

                if (MenuItemList.Any(x=>x.Food.FoodId.Equals(SelectedFood.FoodId))) {
                    return true;
                }
                return false;

            },(p) => {

                if (IsTakeaway) {
                    if (SelectedTakeaway == null) {
                        MessageBox.Show("Choose a bill first !","Notification",MessageBoxButton.OK,MessageBoxImage.Information);

                    } else {

                        string foodId = SelectedFood != null ? SelectedFood.FoodId : "0";


                        if (BillId != null) {

                            BillInfoDao.Instance.InsertBillInfo(BillId,foodId,Convert.ToInt16(FoodQuantity * (-1)));
                        }

                        LoadMenuTakeawayData(BillId);
                        LoadTakeawayData();
                    }
                }
                else {
                    if (SelectedTable == null) {
                        MessageBox.Show("Choose a table first !","Notification",MessageBoxButton.OK,MessageBoxImage.Information);

                    } else {
                        int quantity = FoodQuantity * (-1);
                        string foodId = SelectedFood != null ? SelectedFood.FoodId : "0";



                        BillInfoDao.Instance.InsertBillInfo(BillId,foodId,Convert.ToInt16(quantity));


                        LoadMenuData(SelectedTable.TableId);
                        LoadTableData();

                    }
                }


            });

            ClearMenuCommand = new RelayCommand<object>((p) => {

            if (BillId != null) {
                    return true;
                }
                return false;

            },(p) => {

                if (IsTakeaway) {
                    if (SelectedTakeaway == null) {
                        MessageBox.Show("Choose a bill first !","Notification",MessageBoxButton.OK,MessageBoxImage.Information);

                    } else {


                        BillInfoDao.Instance.RemoveAllBillInfo(BillId);

                        LoadMenuTakeawayData(BillId);
                        LoadTakeawayData();

                    }
                } else {
                    if (SelectedTable == null) {
                        MessageBox.Show("Choose a table first !","Notification",MessageBoxButton.OK,MessageBoxImage.Information);

                    } else {


                        BillDao.Instance.RemoveBill(BillId,SelectedTable.TableId);

                        LoadMenuData(SelectedTable.TableId);
                        LoadTableData();

                    }
                }



            });

            AddTakeawayCommand = new RelayCommand<object>((p) => {

                return true;

            },(p) => {

                string maxBillId = BillDao.Instance.GetBillIdMax();
                BillDao.Instance.InsertTakeawayBill(maxBillId,AccountDao.Instance.CurrrentUser.AccountId);
                LoadTakeawayData();

            });

            RemoveTakeawayCommand = new RelayCommand<object>((p) => {

                if (BillId != null) {
                    return true;
                }
                return false;


            },(p) => {


                BillDao.Instance.RemoveBill(BillId,AccountDao.Instance.CurrrentUser.AccountId);
                LoadTakeawayData();

            });

            CheckOutCommand = new RelayCommand<object>((p) => {

                if (BillId != null) {
                    return true;
                }
                return false;


            },(p) => {


                if (IsTakeaway) {
                    if (SelectedTakeaway == null) {
                        MessageBox.Show("Choose a bill first !","Notification",MessageBoxButton.OK,MessageBoxImage.Information);

                    } else {

                        if (MenuItemList.Count == 0) {
                            MessageBox.Show($"This table have no item !","Notification",MessageBoxButton.OK,MessageBoxImage.Information);

                        }

                        if (MessageBox.Show($"Do you really want to checkout this takeaway order?","Caution",MessageBoxButton.OKCancel).Equals(MessageBoxResult.OK)) {
                            var check = BillDao.Instance.CheckOut(BillId,Discount,Total,null);


                            if (check) {
                                MessageBox.Show($"Checkout successfully !","Notification",MessageBoxButton.OK,MessageBoxImage.Information);
                                BillDetailView billDetailView = new BillDetailView(BillId);
                                billDetailView.Show();

                            }
                            LoadTakeawayData();
                        }



                    }
                } else {
                    if (SelectedTable == null) {
                        MessageBox.Show("Choose a table first !","Notification",MessageBoxButton.OK,MessageBoxImage.Information);

                    } else {


                        if (MenuItemList.Count == 0) {
                            MessageBox.Show($"This takeaway have no item !","Notification",MessageBoxButton.OK,MessageBoxImage.Information);

                        }

                        if (MessageBox.Show($"Do you really want to checkout this table?","Caution",MessageBoxButton.OKCancel).Equals(MessageBoxResult.OK)) {

                            var check = BillDao.Instance.CheckOut(BillId,Discount,Total,SelectedTable.TableId);
                            if (check) {
                                MessageBox.Show($"Checkout successfully !","Notification",MessageBoxButton.OK,MessageBoxImage.Information);
                                BillDetailView billDetailView = new BillDetailView(BillId);
                                billDetailView.Show();
                            }
                            LoadTableData();
                        }

                    }
                }

            });

            OpenAdminOptionsCommand = new RelayCommand<object>((p) => {

                if (AccountDao.Instance.CurrrentUser.Type != 1) {
                    return false;
                }
                return true;

            },(p) => {
                AdminView adminView = new AdminView();
                WindowVisiable = false;
                adminView.ShowDialog();
                WindowVisiable = true;


            });

            OpenProfileOptionsCommand = new RelayCommand<object>((p) => {

                return true;

            },(p) => {
                AccountActionView accountAction = new AccountActionView(AccountDao.Instance.CurrrentUser);
                WindowVisiable = false;
                accountAction.ShowDialog();
                WindowVisiable = true;


            });

            OpenViewBillsOptionsCommand = new RelayCommand<object>((p) => {

                return true;

            },(p) => {
                BillView billView = new BillView();
                WindowVisiable = false;
                billView.ShowDialog();
                WindowVisiable = true;


            });
        }


    }
}
