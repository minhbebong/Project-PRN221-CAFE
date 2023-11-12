using CafeShopFPT.DAO.TableFoodDao;
using CafeShopFPT.Models;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace CafeShopFPT.DAO.TableFoodDao
{
    public class TablesFoodDao
    {
        private static ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private static TablesFoodDao instance;

        public static TablesFoodDao Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new TablesFoodDao();
                }
                return instance;
            }
            private set => instance = value;
        }

        private TablesFoodDao()
        {
        }

        public List<TableDTO> LoadAllTables(bool isAdmin)
        {

            List<TableDTO> ObjTableList = new List<TableDTO>();
            if (isAdmin)
            {
                var collection = DataProvider.Ins.DB.TableFoods.Where(x => !x.TableId.TrimEnd().Equals("0")).OrderBy(x => x.TableId).ToList();
                foreach (var item in collection)
                {
                    ObjTableList.Add(new TableDTO
                    {
                        TableId = item.TableId,
                        Name = item.Name,
                        Status = item.Status,
                        InUse = item.InUse,
                    });
                }

                return ObjTableList;
            }
            else
            {

                var result = (from table in DataProvider.Ins.DB.TableFoods.Where(x => !x.TableId.TrimEnd().Equals("0"))
                              where table.InUse == true

                              select new TableDTO
                              {
                                  Name = table.Name,
                                  Status = table.Status,
                                  TableId = table.TableId,
                                  InUse = table.InUse,
                              });


                return result.OrderBy(x => x.Name).ToList();

            }
        }

        public bool ChangeTableStatus(string tableId, bool status)
        {
            var table = DataProvider.Ins.DB.TableFoods.Where(x => x.TableId.Equals(tableId)).FirstOrDefault();
            if (table != null)
            {
                table.Status = status;
                DataProvider.Ins.DB.TableFoods.Update(table);
                DataProvider.Ins.SaveChanges();
            }
            return true;

        }
        public string? GetTableIdMax()
        {

            try
            {
                var tableIds = DataProvider.Ins.DB.TableFoods.Select(x => x.TableId).ToList();
                int maxId = -1;
                foreach (var tableId in tableIds)
                {
                    if (Convert.ToInt32(tableId) > maxId)
                    {
                        maxId = Convert.ToInt32(tableId);
                    }
                }
                if (maxId == -1)
                {
                    return (0).ToString();
                }
                else
                {
                    return (maxId + 1).ToString();
                }
            }
            catch (Exception)
            {

                return null;
            }


        }

        public bool AddTable(string tableId, string tableName)
        {

            try
            {
                var newTable = new TableFood
                {
                    TableId = tableId,
                    Name = tableName,
                    Status = false,
                    InUse = false,
                };

                DataProvider.Ins.DB.TableFoods.Add(newTable);
                DataProvider.Ins.SaveChanges();

                return true;
            }
            catch (Exception)
            {
                return false;
                throw;
            }


        }

        public bool UpdateTable(TableDTO table)
        {
            try
            {
                var updatedTable = DataProvider.Ins.DB.TableFoods.Where(x => x.TableId.Equals(table.TableId)).FirstOrDefault();
                if (updatedTable != null)
                {


                    updatedTable.Name = table.Name;
                    updatedTable.Status = table.Status;
                    updatedTable.InUse = table.InUse;

                    DataProvider.Ins.DB.TableFoods.Update(updatedTable);
                    DataProvider.Ins.SaveChanges();
                };

                return true;
            }
            catch (Exception)
            {
                return false;
                throw;
            }
        }

        public bool RemoveTable(string tableId)
        {
            try
            {
                var removeTable = DataProvider.Ins.DB.TableFoods.Where(x => x.TableId.Equals(tableId)).FirstOrDefault();
                if (removeTable != null)
                {
                    DataProvider.Ins.DB.TableFoods.Remove(removeTable);
                    DataProvider.Ins.SaveChanges();
                }
                return true;
            }
            catch (Exception)
            {
                return false;
                throw;
            }

        }
    }
}
