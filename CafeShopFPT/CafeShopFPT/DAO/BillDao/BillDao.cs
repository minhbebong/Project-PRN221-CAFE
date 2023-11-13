using log4net;
using Microsoft.EntityFrameworkCore;
using CafeShopFPT.DAO.TableFoodDao;
using CafeShopFPT.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace CafeShopFPT.DAO.BillDao {
    public class BillDao {
        private static ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private static BillDao instance;

        public static BillDao Instance {
            get {
                if (instance == null) {
                    instance = new BillDao();
                }
                return instance;
            }
            private set => instance = value;
        }

        private BillDao() {
        }

        public string? GetUncheckBillIDByTableID(string tableId) {
            try {
                var billId = DataProvider.Ins.DB.Bills
          .Where(x => x.TableId.TrimEnd().Equals(tableId) && x.Status == 0).Select(x => x.BillId).FirstOrDefault();

                if (string.IsNullOrEmpty(billId)) {
                    return null;
                } else {
                    return billId;
                }
            } catch (Exception) {

                throw;
            }

        }

        public string? GetBillIdMax() {

            try {
                var maxId = DataProvider.Ins.DB.Bills.Max(x => x.BillId);
                if (string.IsNullOrEmpty(maxId)) {
                    return (0).ToString().PadLeft(10,'0');
                } else {
                    return (Convert.ToInt32(maxId) + 1).ToString().PadLeft(10,'0');
                }
            } catch (Exception) {

                return null;
            }


        }

        public bool InsertBill(string billId,string tableId,string accountId) {

            try {
                var newBill = new Models.Bill() {
                    BillId = billId,
                    TableId = tableId,
                    AccountId = accountId,
                    DateCheckIn = DateTime.Now,
                    Status = 0,
                    Discount = 0,
                };

                DataProvider.Ins.DB.Bills.Add(newBill);
                DataProvider.Ins.SaveChanges();
                TablesFoodDao.Instance.ChangeTableStatus(tableId,true);

                return true;
            } catch (Exception) {

                return false;
            }

        }

        public bool InsertTakeawayBill(string billId,string accountId) {

            try {
                var newBill = new Models.Bill() {
                    BillId = billId,
                    TableId = "0",
                    AccountId = accountId,
                    DateCheckIn = DateTime.Now,
                    Status = 0,
                    Discount = 0,
                };

                DataProvider.Ins.DB.Bills.Add(newBill);
                DataProvider.Ins.SaveChanges();

                return true;
            } catch (Exception) {

                return false;
            }

        }

        public bool RemoveTakeawayBill(string billId) {

            try {
                var deletedBill = DataProvider.Ins.DB.Bills.Where(x => x.BillId.Equals(billId)).FirstOrDefault();
                if (deletedBill != null) {
                    var listBillInfo = DataProvider.Ins.DB.BillInfos.Where(x => x.BillId.Equals(billId));

                    DataProvider.Ins.DB.BillInfos.RemoveRange(listBillInfo);
                    DataProvider.Ins.DB.Bills.Remove(deletedBill);
                    DataProvider.Ins.SaveChanges();
                }




                return true;
            } catch (Exception) {

                return false;
            }

        }

        public bool RemoveBill(string billId, string tableId) {

            try {
                var deletedBill =  DataProvider.Ins.DB.Bills.Where(x => x.BillId.Equals(billId)).FirstOrDefault();
                if (deletedBill != null ) {
                    var listBillInfo = DataProvider.Ins.DB.BillInfos.Where(x => x.BillId.Equals(billId));
                    DataProvider.Ins.DB.BillInfos.RemoveRange(listBillInfo);
                    DataProvider.Ins.DB.Bills.Remove(deletedBill);
                    DataProvider.Ins.SaveChanges();
                }


                TablesFoodDao.Instance.ChangeTableStatus(tableId,false);

                return true;
            } catch (Exception) {

                return false;
            }

        }

        public List<BillDTO> LoadAllCheckoutBillByDate(DateTime? fromDate, DateTime? toDate, string? billId, string? accountName) {

            try {
                var checkoutBills = DataProvider.Ins.DB.Bills.Where(x=>x.Status == 1).AsQueryable();
                if (fromDate != null) {
                    checkoutBills = checkoutBills.Where(x => x.DateCheckIn.Date >= ((DateTime)fromDate).Date);
                }

                if (toDate != null) {
                    checkoutBills = checkoutBills.Where(x => x.DateCheckIn.Date <= ((DateTime)toDate).Date);
                }
                if (!string.IsNullOrEmpty(billId) ){
                    checkoutBills = checkoutBills.Where(x => x.BillId.Equals(billId.TrimEnd()));
                }

                if (!string.IsNullOrEmpty(accountName)) {
                    checkoutBills = checkoutBills.Where(x => x.Account.DisplayName.ToLower().Contains(accountName.ToLower()));
                }


                return checkoutBills.Select(x =>new BillDTO {
                    BillId = x.BillId,
                    DateCheckIn= x.DateCheckIn,
                    DateCheckOut= x.DateCheckOut,
                    Account = x.Account,
                    AccountId= x.AccountId,
                    Discount= x.Discount,
                    Status= x.Status,
                    TableId = x.TableId,
                    Table = x.Table,
                    Total = x.Total,
                }).ToList();

            } catch (Exception) {

                return null;
            }

        }

        public List<BillDTO> LoadAllTakeAway() {

            try {
                var takeAwayBill = DataProvider.Ins.DB.Bills.Where(x => x.TableId.Trim().Equals("0") && x.Status.Equals(0))
                    .Select(x => new BillDTO {
                        BillId = x.BillId,
                        TableId= x.TableId,
                        AccountId=x.AccountId,
                        Discount = x.Discount,
                        Status= x.Status,
                    }).ToList();


                return takeAwayBill;
            } catch (Exception) {

                return null;
            }

        }
        public BillDTO GetBill(string billId) {

            try {

                var getBill = (from bill in DataProvider.Ins.DB.Bills.Include(x=>x.Account)
                           where bill.BillId.Equals(billId) && bill.Status.Equals(1)
                           select bill).FirstOrDefault();





            var billde = new BillDTO {
                BillId = getBill.BillId,
                TableId = getBill.TableId,
                AccountId = getBill.AccountId,
                DateCheckIn= getBill.DateCheckIn,
                DateCheckOut= getBill.DateCheckOut,
                Discount = getBill.Discount,
                Status = getBill.Status,
                Total = getBill.Total,

                Account = getBill.Account
            };

                //var getBill = DataProvider.Ins.DB.Bills.Where(x => x.BillId.Equals(billId) && x.Status.Equals(1))
                //    .Select(x => new BillDTO {


                //    }).AsNoTracking().SingleOrDefault();


                return billde;
            } catch (Exception) {

                return null;
            }

        }


        public bool CheckOut(string billid,int discount,float finalTotalPrice, string? tableId) {

            try {
                var checkoutBill = DataProvider.Ins.DB.Bills.Where(x => x.BillId.Equals(billid) && x.Status.Equals(0)).SingleOrDefault();
                if (checkoutBill != null) {
                    checkoutBill.DateCheckOut = DateTime.Now;
                    checkoutBill.Status = 1;
                    checkoutBill.Discount = (byte)discount;
                    checkoutBill.Total = (decimal?)finalTotalPrice;

                    DataProvider.Ins.DB.Bills.Update(checkoutBill);
                    DataProvider.Ins.DB.SaveChanges();
                }

                var utBill = DataProvider.Ins.DB.Bills.Where(x => x.BillId.Equals(billid)).SingleOrDefault();
                if (tableId != null) {
                    TablesFoodDao.Instance.ChangeTableStatus(tableId,false);

                }

                return true;
            } catch (Exception) {

                return false;
            }
        }

    }

}


