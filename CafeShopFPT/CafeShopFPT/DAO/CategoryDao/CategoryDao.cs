using log4net;
using CafeShopFPT.DAO.TableFoodDao;
using CafeShopFPT.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CafeShopFPT.DAO.CategoryDao {
    public class CategoryDao {
        private static ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private static CategoryDao instance;

        public static CategoryDao Instance {
            get {
                if (instance == null) {
                    instance = new CategoryDao();
                }
                return instance;
            }
            private set => instance = value;
        }

        private CategoryDao() {
        }

        public List<CategoryDTO> LoadAllCategories() {

              var   ObjTableList = from category in DataProvider.Ins.DB.Categories
                                 select new CategoryDTO {
                                     CategoryId = category.CategoryId,
                                     Name = category.Name,
                                 };
            return ObjTableList.ToList();
                }


        public string? GetCategoryIdMax() {

            try {
                var maxId = DataProvider.Ins.DB.Categories.Max(x => x.CategoryId);
                if (string.IsNullOrEmpty(maxId)) {
                    return (0).ToString();
                } else {
                    return (Convert.ToInt32(maxId) + 1).ToString();
                }
            } catch (Exception) {

                return null;
            }


        }
        public bool AddCategory(string categoryId, string name) {
            try {

            var newCategory = new Category { CategoryId= categoryId, Name = name };

            DataProvider.Ins.DB.Categories.Add(newCategory);
                DataProvider.Ins.DB.SaveChanges();

            return true;
            } catch (Exception) {

                return false;
            }
        }


        public bool RemoveCategory(string categoryId) {
            try {

                var category = DataProvider.Ins.DB.Categories.Where(x => x.CategoryId.Equals(categoryId)).FirstOrDefault();
                if (category != null) {
                    var foods = DataProvider.Ins.DB.Foods.Where(x => x.CategoryId.Equals(categoryId)).ToList();
                    if (foods.Count != 0) {
                        foreach (var food in foods) {
                            food.CategoryId = "0";
                        }
                        DataProvider.Ins.DB.Foods.UpdateRange(foods);
                    }
                    DataProvider.Ins.DB.Categories.Remove(category);
                    DataProvider.Ins.DB.SaveChanges();
                }

                return true;
            } catch (Exception) {

                return false;
            }
        }


    }
        }


