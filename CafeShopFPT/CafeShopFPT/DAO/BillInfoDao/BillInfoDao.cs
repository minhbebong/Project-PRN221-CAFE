using CafeShopFPT.DAO.BillInfoDao;
using CafeShopFPT.DAO.FoodDao;
using CafeShopFPT.DAO.TableFoodDao;
using CafeShopFPT.Models;
using log4net;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace CafeShopFPT.DAO.BillInfoDao
{
    public class BillInfoDao
    {
        private static ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private static BillInfoDao instance;

        public static BillInfoDao Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new BillInfoDao();
                }
                return instance;
            }
            private set => instance = value;
        }

        private BillInfoDao()
        {
        }

        public string InsertBillInfo(string billId, string foodId, short quantity)
        {

            try
            {
                var getBillIdExist = DataProvider.Ins.DB.BillInfos
                .Where(x => x.BillId.TrimEnd().Equals(billId)
                && x.FoodId.TrimEnd().Equals(foodId))
                .FirstOrDefault();

                if (!string.IsNullOrEmpty(getBillIdExist?.BillId))
                {
                    var newQuantity = (short)(getBillIdExist.Quantity + quantity);

                    if (newQuantity > 0)
                    {
                        getBillIdExist.Quantity = newQuantity;
                        DataProvider.Ins.DB.BillInfos.Update(getBillIdExist);
                        DataProvider.Ins.DB.SaveChanges();

                    }
                    else
                    {
                        DataProvider.Ins.DB.BillInfos.Remove(getBillIdExist);
                        DataProvider.Ins.DB.SaveChanges();
                        if (!DataProvider.Ins.DB.BillInfos.Any(x => x.BillId.Equals(billId)))
                        {
                            var bill = DataProvider.Ins.DB.Bills.Where(x => x.BillId.Equals(billId)).First();
                            DataProvider.Ins.DB.Bills.Remove(bill);

                            DataProvider.Ins.DB.SaveChanges();
                            TablesFoodDao.Instance.ChangeTableStatus(bill.TableId, false);
                        }
                    }

                }
                else
                {
                    var newBillInfo = new Models.BillInfo() { BillId = billId, FoodId = foodId, Quantity = quantity };
                    DataProvider.Ins.DB.BillInfos.Add(newBillInfo);
                    DataProvider.Ins.DB.SaveChanges();

                }
                return billId;
            }
            catch (System.Exception)
            {

                return null;
            }
        }



        public List<MenuItemDTO> GetListFoodOrder(string tableId)
        {
            try
            {
                var listMenu = from billInfo in DataProvider.Ins.DB.BillInfos
                               join bill in DataProvider.Ins.DB.Bills
                               on billInfo.BillId equals bill.BillId
                               into bill_billInfo
                               from bill in bill_billInfo
                               join food in DataProvider.Ins.DB.Foods
                               on billInfo.FoodId equals food.FoodId
                               where bill.TableId.Equals(tableId) && bill.Status.Equals(0)
                               select new MenuItemDTO
                               {
                                   Food = new FoodDTO { FoodId = food.FoodId, CategoryId = food.CategoryId, FoodName = food.FoodName, Price = food.Price, ImgPath = food.ImgPath },
                                   Price = food.Price,
                                   Quantity = billInfo.Quantity,
                                   Total = food.Price * billInfo.Quantity,
                               };

                return listMenu.ToList();
            }
            catch (System.Exception)
            {

                return new List<MenuItemDTO>();
            }

        }

        public bool RemoveAllBillInfo(string billId)
        {

            try
            {
                var deletedBill = DataProvider.Ins.DB.Bills.Where(x => x.BillId.Equals(billId)).FirstOrDefault();
                if (deletedBill != null)
                {
                    var listBillInfo = DataProvider.Ins.DB.BillInfos.Where(x => x.BillId.Equals(billId));
                    DataProvider.Ins.DB.BillInfos.RemoveRange(listBillInfo);

                    DataProvider.Ins.SaveChanges();
                }


                return true;
            }
            catch (Exception)
            {

                return false;
            }

        }

        public bool RemoveAllBillInfoByFoodId(string foodId)
        {

            try
            {


                var listBillInfo = DataProvider.Ins.DB.BillInfos.Where(x => x.FoodId.Equals(foodId));
                if (listBillInfo != null)
                {
                    DataProvider.Ins.DB.BillInfos.RemoveRange(listBillInfo);

                    DataProvider.Ins.SaveChanges();
                }




                return true;
            }
            catch (Exception)
            {

                return false;
            }

        }

        public List<MenuItemDTO> GetListFoodTakeaway(string billId)
        {
            try
            {
                var listMenu = from billInfo in DataProvider.Ins.DB.BillInfos
                               join bill in DataProvider.Ins.DB.Bills
                               on billInfo.BillId equals bill.BillId
                               into bill_billInfo
                               from bill in bill_billInfo
                               join food in DataProvider.Ins.DB.Foods
                               on billInfo.FoodId equals food.FoodId
                               where bill.TableId.Equals("0") && bill.BillId.Equals(billId)
                               select new MenuItemDTO
                               {
                                   Food = new FoodDTO { FoodId = food.FoodId, CategoryId = food.CategoryId, FoodName = food.FoodName, Price = food.Price, ImgPath = food.ImgPath },
                                   Price = food.Price,
                                   Quantity = billInfo.Quantity,
                                   Total = food.Price * billInfo.Quantity,
                               };

                return listMenu.ToList();
            }
            catch (System.Exception)
            {

                return new List<MenuItemDTO>();
            }

        }

        public List<MenuItemDTO> GetListFoodOfBill(string billId)
        {
            try
            {
                var listMenu = from billInfo in DataProvider.Ins.DB.BillInfos
                               join bill in DataProvider.Ins.DB.Bills
                               on billInfo.BillId equals bill.BillId
                               into bill_billInfo
                               from bill in bill_billInfo
                               join food in DataProvider.Ins.DB.Foods
                               on billInfo.FoodId equals food.FoodId
                               where bill.BillId.Equals(billId)
                               select new MenuItemDTO
                               {
                                   Food = new FoodDTO { FoodId = food.FoodId, CategoryId = food.CategoryId, FoodName = food.FoodName, Price = food.Price, ImgPath = food.ImgPath },
                                   Price = food.Price,
                                   Quantity = billInfo.Quantity,
                                   Total = food.Price * billInfo.Quantity,
                               };

                return listMenu.ToList();
            }
            catch (System.Exception)
            {

                return new List<MenuItemDTO>();
            }

        }
    }
}


