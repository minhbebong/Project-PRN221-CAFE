using CafeShopFPT.DAO.AccountsDao;
using CafeShopFPT.DAO.BillDao;
using CafeShopFPT.DAO.BillInfoDao;
using CafeShopFPT.DAO.CategoryDao;
using CafeShopFPT.DAO.FoodDao;
using CafeShopFPT.DAO.TableFoodDao;
using CafeShopFPT.Views;
using ClosedXML.Excel;
using Microsoft.Win32;
using ProductCURD01.ViewModel;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CafeShopFPT.ViewModels.AdminScreen
{
    public class AdminVM : BaseVM
    {

        #region Property
        private TabItem _selectedTab;
        public TabItem SelectedTab
        {
            get
            {
                return _selectedTab;
            }
            set
            {
                _selectedTab = value; OnPropertyChanged();
                var selectTab = SelectedTab.Name;
                switch (selectTab)
                {
                    case "tpFood":
                        if (DisplayFoodList == null)
                        {
                            LoadCategoriesData();
                            LoadFoodData();
                        }
                        break;

                    case "tpCategory":
                        if (CategoriesList == null)
                        {
                            LoadCategoriesData();
                        }
                        break;

                    case "tpTable":
                        LoadTableData();
                        break;

                    case "tpBill":
                        if (BillList == null)
                        {
                            LoadBillData();
                        }
                        break;

                    case "tpAccount":
                        if (AccountList == null)
                        {
                            LoadAccountData();
                        }
                        break;
                    default:


                        break;
                }

            }
        }



        #region Property FoodTab

        private ObservableCollection<AccountDTO> _accountList;
        public ObservableCollection<AccountDTO> AccountList
        {
            get
            {
                return _accountList;
            }
            set
            {
                _accountList = value; OnPropertyChanged();
            }
        }

        private ObservableCollection<FoodDTO> _baseFoodList;
        public ObservableCollection<FoodDTO> BaseFoodList
        {
            get
            {
                return _baseFoodList;
            }
            set
            {
                _baseFoodList = value; OnPropertyChanged();
            }
        }

        private ObservableCollection<FoodDTO> _displayFoodList;
        public ObservableCollection<FoodDTO> DisplayFoodList
        {
            get
            {
                return _displayFoodList;
            }
            set
            {
                _displayFoodList = value; OnPropertyChanged();
            }
        }

        private string _searchFoodQuery;
        public string SearchFoodQuery
        {
            get
            {
                return _searchFoodQuery;
            }
            set
            {
                _searchFoodQuery = value; OnPropertyChanged();
                SearchFoodData();

            }
        }

        private CategoryDTO _selectedCategory;
        public CategoryDTO SelectedCategory
        {
            get
            {
                return _selectedCategory;
            }
            set
            {
                _selectedCategory = value; OnPropertyChanged();
                SearchFoodData();

            }
        }

        private ObservableCollection<CategoryDTO> _categoriesList;
        public ObservableCollection<CategoryDTO> CategoriesList
        {
            get
            {
                return _categoriesList;
            }
            set
            {
                _categoriesList = value; OnPropertyChanged();
            }
        }


        private string _categoryName;
        public string CategoryName
        {
            get
            {
                return _categoryName;
            }
            set
            {
                _categoryName = value; OnPropertyChanged();

            }
        }
        #endregion

        #region TableTab
        private ObservableCollection<TableDTO> _tablesList;
        public ObservableCollection<TableDTO> TablesList
        {
            get
            {
                return _tablesList;
            }
            set
            {
                _tablesList = value; OnPropertyChanged();
            }
        }

        private ObservableCollection<BillDTO> _billList = new ObservableCollection<BillDTO>();
        public ObservableCollection<BillDTO> BillList
        {
            get
            {
                return _billList;
            }
            set
            {
                _billList = value; OnPropertyChanged();

                if (BillList.Count > 0)
                {
                    Revenue = BillList.Sum(x => x.Total);
                }
            }
        }

        private BillDTO _selectedBill;
        public BillDTO SelectedBill
        {
            get
            {
                return _selectedBill;
            }
            set
            {
                _selectedBill = value; OnPropertyChanged();


            }
        }

        private TableDTO _selectedTable;
        public TableDTO SelectedTable
        {
            get
            {
                return _selectedTable;
            }
            set
            {
                _selectedTable = value; OnPropertyChanged();


            }
        }

        private string _addTableName;
        public string AddTableName
        {
            get
            {
                return _addTableName;
            }
            set
            {
                _addTableName = value; OnPropertyChanged();

            }
        }

        private DateTime? _fromBillDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
        public DateTime? FromBillDate
        {
            get
            {
                return _fromBillDate;
            }
            set
            {
                if (ToBillDate != null)
                {
                    if (ToBillDate.Value < value)
                    {
                        MessageBox.Show("To date cannot smaller than from date!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                }
                _fromBillDate = value; OnPropertyChanged();

            }
        }

        private DateTime? _toBillDate = DateTime.Now;
        public DateTime? ToBillDate
        {
            get
            {
                return _toBillDate;
            }
            set
            {
                if (FromBillDate != null)
                {
                    if (FromBillDate.Value > value)
                    {
                        MessageBox.Show("From date cannot bigger than To date!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                }
                _toBillDate = value; OnPropertyChanged();

            }
        }

        private string _searchBillId;
        public string SearchBillId
        {
            get
            {
                return _searchBillId;
            }
            set
            {
                _searchBillId = value; OnPropertyChanged();

            }
        }

        private string _searchAccountName;
        public string SearchAccountName
        {
            get
            {
                return _searchAccountName;
            }
            set
            {
                _searchAccountName = value; OnPropertyChanged();

            }
        }
        private decimal? _revenue;
        public decimal? Revenue
        {
            get
            {
                return _revenue;
            }
            set
            {
                _revenue = value; OnPropertyChanged();

            }
        }



        #endregion

        #endregion

        #region Function
        #region Food

        private void LoadFoodData()
        {
            DisplayFoodList = new ObservableCollection<FoodDTO>(FoodDao.Instance.LoadAllFood());
            BaseFoodList = DisplayFoodList;
        }

        private void LoadCategoriesData()
        {
            CategoriesList = new ObservableCollection<CategoryDTO>(CategoryDao.Instance.LoadAllCategories());
        }

        private void SearchFoodData()
        {
            var tempListFood = BaseFoodList;
            if (SelectedCategory != null)
            {
                tempListFood = new ObservableCollection<FoodDTO>(tempListFood.Where(x => x.CategoryId.Equals(SelectedCategory.CategoryId)));
            }

            if (!string.IsNullOrEmpty(SearchFoodQuery))
            {
                tempListFood = new ObservableCollection<FoodDTO>(tempListFood.Where(x => x.FoodName.ToLower().Contains(SearchFoodQuery.ToLower())));
            }

            DisplayFoodList = tempListFood;
        }

        private void LoadAccountData()
        {
            AccountList = new ObservableCollection<AccountDTO>(AccountDao.Instance.LoadAllAccount());

        }

        private void LoadBillData()
        {
            BillList = new ObservableCollection<BillDTO>(BillDao.Instance.LoadAllCheckoutBillByDate(FromBillDate, ToBillDate, SearchBillId, SearchAccountName));
        }


        #endregion

        #region Table
        private void LoadTableData()
        {
            TablesList = new ObservableCollection<TableDTO>(TablesFoodDao.Instance.LoadAllTables(true));

        }
        #endregion


        #endregion

        #region Command

        public ICommand ResetFoodFilterCommand
        {
            get; set;
        }

        public ICommand AddFoodCommand
        {
            get; set;
        }

        public ICommand EditFoodCommand
        {
            get; set;
        }

        public ICommand RemoveFoodCommand
        {
            get; set;
        }

        public ICommand AddCategoryCommand
        {
            get; set;
        }

        public ICommand EditCategoryCommand
        {
            get; set;
        }

        public ICommand RemoveCategoryCommand
        {
            get; set;
        }

        public ICommand TableSelectCommand
        {
            get; set;
        }

        public ICommand UpdateTableCommand
        {
            get; set;
        }

        public ICommand AddTableCommand
        {
            get; set;
        }

        public ICommand RemoveTableCommand
        {
            get; set;
        }

        public ICommand UpdateAccountCommand
        {
            get; set;
        }

        public ICommand AddAccountCommand
        {
            get; set;
        }

        public ICommand RemoveAccountCommand
        {
            get; set;
        }

        public ICommand SearchBillCommand
        {
            get; set;
        }


        public ICommand ViewBillDetailCommand
        {
            get; set;
        }

        public ICommand ExportToExcelCommand
        {
            get; set;
        }

        #endregion

        public AdminVM()
        {


            ResetFoodFilterCommand = new RelayCommand<object>((p) => {

                if (SelectedCategory != null || !string.IsNullOrEmpty(SearchFoodQuery))
                {
                    return true;
                }
                return false;

            }, (p) => {

                SelectedCategory = null;
                SearchFoodQuery = null;


            });


            AddFoodCommand = new RelayCommand<object>((p) => {

                return true;

            }, (p) => {

                FoodActionView foodActionView = new FoodActionView();
                foodActionView.ShowDialog();
                LoadFoodData();

            });

            EditFoodCommand = new RelayCommand<object>((p) => {

                return true;

            }, (p) => {

                FoodDTO food = (p as Button).DataContext as FoodDTO;
                if (food != null)
                {
                    FoodActionView foodActionView = new FoodActionView(food);
                    foodActionView.ShowDialog();
                    LoadFoodData();
                }



            });

            RemoveFoodCommand = new RelayCommand<object>((p) => {

                return true;

            }, (p) => {

                FoodDTO food = (p as Button).DataContext as FoodDTO;
                if (food != null)
                {
                    if (MessageBox.Show($"Remove this food also remove all bill infomation invole, do you want to continue?", "Caution", MessageBoxButton.OKCancel, MessageBoxImage.Warning).Equals(MessageBoxResult.OK))
                    {
                        var check = BillInfoDao.Instance.RemoveAllBillInfoByFoodId(food.FoodId);
                        FoodDao.Instance.RemoveFood(food);

                        if (check)
                        {
                            MessageBox.Show($"Remove food successfully !", "Notification", MessageBoxButton.OK, MessageBoxImage.Information);

                        }
                        LoadFoodData();

                    }

                }

            });

            AddCategoryCommand = new RelayCommand<object>((p) => {

                if (string.IsNullOrEmpty(CategoryName))
                {
                    return false;
                }
                return true;

            }, (p) => {
                var maxId = CategoryDao.Instance.GetCategoryIdMax();

                if (CategoryDao.Instance.AddCategory(maxId, CategoryName))
                {
                    MessageBox.Show("Category add successfully!", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Category add fail!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                LoadCategoriesData();

            });


            EditCategoryCommand = new RelayCommand<object>((p) => {

                return true;

            }, (p) => {
                CategoryDTO category = (p as Button).DataContext as CategoryDTO;
                if (category != null)
                {
                    CategoryActionView categoryActionView = new CategoryActionView(category.CategoryId);
                    categoryActionView.ShowDialog();
                }

            });

            RemoveCategoryCommand = new RelayCommand<object>((p) => {

                return true;

            }, (p) => {
                CategoryDTO category = (p as Button).DataContext as CategoryDTO;
                if (!category.CategoryId.Trim().Equals("0"))
                {
                    if (CategoryDao.Instance.RemoveCategory(category.CategoryId))
                    {
                        MessageBox.Show("Category remove successfully!", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show("Category remove fail!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Cannot remove this!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }

                LoadCategoriesData();
            });

            TableSelectCommand = new RelayCommand<object>((p) => {

                return true;

            }, (p) => {
                SelectedTable = ((p as Button).DataContext as TableDTO);


            });

            AddTableCommand = new RelayCommand<object>((p) => {
                if (String.IsNullOrEmpty(AddTableName))
                {
                    return false;
                }
                return true;

            }, (p) => {
                if (TablesFoodDao.Instance.AddTable(TablesFoodDao.Instance.GetTableIdMax(), AddTableName))
                {
                    MessageBox.Show("Table added successfully!", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Table added fail!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                LoadTableData();

            });

            UpdateTableCommand = new RelayCommand<object>((p) => {
                if (SelectedTable != null)
                {
                    return true;
                }
                return false;

            }, (p) => {
                if (TablesFoodDao.Instance.UpdateTable(SelectedTable))
                {
                    MessageBox.Show("Table update successfully!", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Table update fail!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                LoadTableData();


            });

            RemoveTableCommand = new RelayCommand<object>((p) => {

                if (SelectedTable != null)
                {
                    return true;
                }
                return false;

            }, (p) => {

                if (TablesFoodDao.Instance.RemoveTable(SelectedTable.TableId))
                {
                    MessageBox.Show("Table remove successfully!", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Table remove fail!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                LoadTableData();

            });


            AddAccountCommand = new RelayCommand<object>((p) => {

                return true;

            }, (p) => {
                AccountActionView accountActionView = new AccountActionView();
                accountActionView.ShowDialog();
                LoadAccountData();

            });

            UpdateAccountCommand = new RelayCommand<object>((p) => {

                return true;


            }, (p) => {
                var account = ((p as Button).DataContext as AccountDTO);

                AccountActionView accountActionView = new AccountActionView(account);
                accountActionView.ShowDialog();
                LoadAccountData();


            });

            RemoveAccountCommand = new RelayCommand<object>((p) => {


                return true;


            }, (p) => {
                var account = ((p as Button).DataContext as AccountDTO);

                if (AccountDao.Instance.RemoveAccount(account.AccountId))
                {
                    MessageBox.Show("Account remove successfully!", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Account remove fail!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                LoadAccountData();

            });

            SearchBillCommand = new RelayCommand<object>((p) => {


                return true;


            }, (p) => {
                LoadBillData();

            });

            ViewBillDetailCommand = new RelayCommand<object>((p) => {


                return true;


            }, (p) => {
                BillDTO bill = (p as Button).DataContext as BillDTO;
                BillDetailView billDetailView = new BillDetailView(bill.BillId);

                billDetailView.ShowDialog();

            });

            ExportToExcelCommand = new RelayCommand<object>((p) => {

                if (BillList.Count > 0)
                {
                    return true;
                }
                return false;


            }, (p) => {

                SaveFileDialog sfd = new SaveFileDialog();
                System.Globalization.CultureInfo cultureinfo =
        new System.Globalization.CultureInfo("vi-VN");

                sfd.Filter = "Excel Files|*.xlsx;*.xlsm;*.xltx;*.xltm";
                sfd.FileName = $"Doanh_thu_thang_{DateTime.Now.Month}.xlsx";
                if (sfd.ShowDialog() == true)
                {

                    using (var wb = new XLWorkbook())
                    {
                        wb.Worksheets.Add("Sheet1");
                        var wbSheet = wb.Worksheet(1);

                        var title1 = wbSheet.Cell("A1");
                        title1.Value = $"BÁO CÁO DOANH THU";
                        title1.Style.Font.Bold = true;
                        title1.Style.Font.FontName = "Times New Roman";
                        title1.Style.Font.FontSize = 16;
                        title1.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                        title1.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                        wbSheet.Range("A1:F1").Merge();

                        var title2 = wbSheet.Cell("A2");
                        title2.Value = $"Từ ngày {FromBillDate.Value.Date.ToString("dd/MM/yyyy")}      Đến ngày {ToBillDate.Value.Date.ToString("dd/MM/yyyy")}";
                        title2.Style.Font.FontName = "Times New Roman";
                        title2.Style.Font.FontSize = 12;
                        title2.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                        title2.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                        wbSheet.Range("A2:F2").Merge();

                        var data = wbSheet.Cell("A5");
                        data.InsertTable(BillList);
                        wbSheet.Columns("A", "O").AdjustToContents();

                        var total = wbSheet.Cell(wbSheet.LastRowUsed().RowBelow().RowNumber(), "E");
                        var total2 = wbSheet.Cell(wbSheet.LastRowUsed().RowBelow().RowNumber(), "F");

                        wbSheet.Range(total, total2).Merge();
                        var total3 = wbSheet.Cell(wbSheet.LastRowUsed().RowBelow().RowNumber(), "G");
                        total.Value = $"Tổng doanh thu:";
                        total.Style.Font.FontName = "Times New Roman";
                        total.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                        total.Style.Font.Bold = true;
                        total.Style.Font.FontSize = 16;
                        total3.Value = $"{((decimal)Revenue).ToString("N2")} VNĐ";

                        wb.SaveAs(sfd.FileName);
                        new Process
                        {
                            StartInfo = new ProcessStartInfo(sfd.FileName)
                            {
                                UseShellExecute = true
                            }
                        }.Start();

                    }

                }
                else
                {
                    MessageBox.Show("No bill exists in the table!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                }
            }


            );
        }
    }
}



