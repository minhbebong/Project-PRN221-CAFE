using DocumentFormat.OpenXml.Math;
using DocumentFormat.OpenXml.Presentation;
using CafeShopFPT.DAO.BillDao;
using CafeShopFPT.Views;
using ProductCURD01.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CafeShopFPT.ViewModels.ManagerScreen {
    public class BillViewVM :BaseVM{

        #region Property

        private ObservableCollection<BillDTO> _billList = new ObservableCollection<BillDTO>();
        public ObservableCollection<BillDTO> BillList {
            get {
                return _billList;
            }
            set {
                _billList = value; OnPropertyChanged();

                if (BillList.Count > 0) {
                    Revenue = BillList.Sum(x => x.Total);
                }
            }
        }

        private DateTime? _fromBillDate = new DateTime(DateTime.Now.Year,DateTime.Now.Month,1);
        public DateTime? FromBillDate {
            get {
                return _fromBillDate;
            }
            set {
                if (ToBillDate != null) {
                    if (ToBillDate.Value < value) {
                        MessageBox.Show("To date cannot smaller than from date!","Error",MessageBoxButton.OK,MessageBoxImage.Error);
                        return;
                    }
                }
                _fromBillDate = value; OnPropertyChanged();

            }
        }

        private DateTime? _toBillDate = DateTime.Now;
        public DateTime? ToBillDate {
            get {
                return _toBillDate;
            }
            set {
                if (FromBillDate != null) {
                    if (FromBillDate.Value > value) {
                        MessageBox.Show("From date cannot bigger than To date!","Error",MessageBoxButton.OK,MessageBoxImage.Error);
                        return;
                    }
                }
                _toBillDate = value; OnPropertyChanged();

            }
        }

        private string _searchBillId;
        public string SearchBillId {
            get {
                return _searchBillId;
            }
            set {
                _searchBillId = value; OnPropertyChanged();

            }
        }

        private string _searchAccountName;
        public string SearchAccountName {
            get {
                return _searchAccountName;
            }
            set {
                _searchAccountName = value; OnPropertyChanged();

            }
        }
        private decimal? _revenue;

        public decimal? Revenue {
            get {
                return _revenue;
            }
            set {
                _revenue = value; OnPropertyChanged();

            }
        }

        #endregion

        #region Function
        private void LoadBillData() {
            BillList = new ObservableCollection<BillDTO>(BillDao.Instance.LoadAllCheckoutBillByDate(FromBillDate,ToBillDate,SearchBillId,SearchAccountName));

        }
        #endregion

        #region Command
        public ICommand SearchBillCommand {
            get; set;
        }


        public ICommand ViewBillDetailCommand {
            get; set;
        }
        #endregion

        public BillViewVM() {

            ViewBillDetailCommand = new RelayCommand<object>((p) => {


                return true;


            },(p) => {
                BillDTO bill = (p as Button).DataContext as BillDTO;
                BillDetailView billDetailView = new BillDetailView(bill.BillId);

                billDetailView.ShowDialog();

            });

            SearchBillCommand = new RelayCommand<object>((p) => {


                return true;


            },(p) => {
                LoadBillData();

            });
        }


    }
}
