using log4net;
using Microsoft.EntityFrameworkCore;
using CafeShopFPT.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CafeShopFPT.DAO.FoodDao {
    public class FoodDao {
        private static ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private static FoodDao instance;

        public static FoodDao Instance {
            get {
                if (instance == null) {
                    instance = new FoodDao();
                }
                return instance;
            }
            private set => instance = value;
        }

        private FoodDao() {
        }

        public List<FoodDTO> LoadAllFoodByCategoryId(string categoryId) {
            try {
                var ObjTableList = from Food in DataProvider.Ins.DB.Foods
                                   where Food.CategoryId.Trim().Equals(categoryId)
                                   select new FoodDTO {
                                       FoodId = Food.FoodId,
                                       FoodName = Food.FoodName,
                                       CategoryId = categoryId,
                                       ImgPath = Food.ImgPath,
                                       Price = Food.Price,
                                   };
                return ObjTableList.ToList();
            } catch (System.Exception) {

                throw;
            }

                }


        public List<FoodDTO> LoadAllFood() {
            try {
                var ObjTableList = from Food in DataProvider.Ins.DB.Foods.Include(x=>x.Category)
                                   select new FoodDTO {
                                       FoodId = Food.FoodId,
                                       FoodName = Food.FoodName,
                                       CategoryId = Food.CategoryId,
                                       ImgPath = Food.ImgPath,
                                       Price = Food.Price,
                                       Category= Food.Category,
                                   };
                return ObjTableList.ToList();
            } catch (System.Exception) {

                throw;
            }
        }

        public string? GetFoodIdMax() {

            try {
                var maxId = DataProvider.Ins.DB.Foods.Max(x => x.FoodId);
                if (string.IsNullOrEmpty(maxId)) {
                    return (0).ToString().PadLeft(5,'0');
                } else {
                    return (Convert.ToInt32(maxId) + 1).ToString().PadLeft(5,'0');
                }
            } catch (Exception) {

                return null;
            }


        }


        public bool AddFood(string foodId,string foodName, string categoryId, decimal price, string imgPath) {
            try {


                var newFood = new Food {
                    FoodId = foodId,
                    FoodName = foodName,
                    CategoryId = categoryId,
                    ImgPath = System.IO.Path.GetFileName(imgPath),
                    Price = price,

                };
                DataProvider.Ins.DB.Foods.Add(newFood);
                DataProvider.Ins.SaveChanges();

                return true;
            } catch (System.Exception) {

                return false;
                throw;
            }
        }

        public bool UpdateFood(FoodDTO food) {
            try {


                var updateFood = DataProvider.Ins.DB.Foods.Where(x => x.FoodId.Equals(food.FoodId)).FirstOrDefault();
                if (updateFood != null) {
                    updateFood.FoodName = food.FoodName;
                    updateFood.CategoryId = food.CategoryId;
                    updateFood.ImgPath = System.IO.Path.GetFileName(food.ImgPath);
                    updateFood.Price = food.Price;
                }

                DataProvider.Ins.DB.Foods.Update(updateFood);
                DataProvider.Ins.SaveChanges();

                return true;
        } catch (System.Exception) {

                return false;
                throw;
            }
}

        public bool RemoveFood(FoodDTO food) {
            try {


                var removeFood = DataProvider.Ins.DB.Foods.Where(x => x.FoodId.Equals(food.FoodId)).FirstOrDefault();
                if (removeFood != null) {
                    DataProvider.Ins.DB.Foods.Remove(removeFood);
                    DataProvider.Ins.SaveChanges();
                }



                return true;
            } catch (System.Exception) {

                return false;
                throw;
            }
        }

        public bool UpdateFoodCategory(string categoryId,string foodId) {
            try {


                var updateFood = DataProvider.Ins.DB.Foods.Where(x => x.FoodId.Equals(foodId)).FirstOrDefault();
                if (updateFood != null) {

                    updateFood.CategoryId = categoryId;
                    DataProvider.Ins.DB.Foods.Update(updateFood);
                    DataProvider.Ins.SaveChanges();
                }



                return true;
            } catch (System.Exception) {

                return false;
                throw;
            }

        }
    }
        }


