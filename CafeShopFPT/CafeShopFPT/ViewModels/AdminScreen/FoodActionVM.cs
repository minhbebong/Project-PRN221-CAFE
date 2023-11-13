using Microsoft.Win32;
using ProductCURD01.ViewModel;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Diagnostics;
using System.IO;
using DocumentFormat.OpenXml.Wordprocessing;
using CafeShopFPT.DAO.CategoryDao;
using CafeShopFPT.DAO.FoodDao;
using CafeShopFPT.LogUlti;

namespace CafeShopFPT.ViewModels.AdminScreen
{
    public class FoodActionVM : BaseVM
    {

        #region Property
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

            }
        }

        private string _foodName;
        public string FoodName
        {
            get
            {
                return _foodName;
            }
            set
            {
                _foodName = value; OnPropertyChanged();


            }
        }

        private FoodDTO _updatedFood;
        public FoodDTO UpdatedFood
        {
            get
            {
                return _updatedFood;
            }
            set
            {
                _updatedFood = value;


            }
        }

        private string _foodImgPath;
        public string FoodImgPath
        {
            get
            {
                return _foodImgPath;
            }
            set
            {
                _foodImgPath = value; OnPropertyChanged();


            }
        }

        private decimal _foodPrice;
        public decimal FoodPrice
        {
            get
            {
                return _foodPrice;
            }
            set
            {
                _foodPrice = value; OnPropertyChanged();


            }
        }

        #endregion


        #region Function
        private void LoadCategoriesData()
        {
            CategoriesList = new ObservableCollection<CategoryDTO>(CategoryDao.Instance.LoadAllCategories());
        }


        private void LoadFoodData(FoodDTO food)
        {

            FoodName = food.FoodName;
            FoodPrice = food.Price;
            FoodImgPath = food.ImgPath;
            SelectedCategory = CategoriesList.Where(x => x.CategoryId.Equals(food.Category.CategoryId)).First();
        }


        #endregion


        #region Command
        public ICommand UploadFoodImageCommand
        {
            get; set;
        }

        public ICommand ResetFoodCommand
        {
            get; set;
        }


        public ICommand SaveFoodCommand
        {
            get; set;
        }
        #endregion



        public FoodActionVM(FoodDTO? food)
        {
            LoadCategoriesData();

            var currFood = food;
            if (food != null)
            {
                LoadFoodData(currFood);

            }


            ResetFoodCommand = new RelayCommand<object>((p) => {
                if (!string.IsNullOrEmpty(FoodName) || SelectedCategory != null)
                {
                    return true;
                }
                return false;

            }, (p) => {
                if (currFood != null)
                {
                    LoadFoodData(currFood);
                }
                else
                {
                    FoodName = null;
                    SelectedCategory = null;
                    FoodPrice = 0;
                    FoodImgPath = null;
                }


            });

            SaveFoodCommand = new RelayCommand<object>((p) => {
                if (!string.IsNullOrEmpty(FoodName) || SelectedCategory != null)
                {
                    return true;
                }
                return false;

            }, (p) => {

                Window thisWindow = p as Window;
                if (food != null)
                {

                    UpdatedFood = new FoodDTO
                    {

                        CategoryId = SelectedCategory.CategoryId,
                        FoodId = food.FoodId,
                        FoodName = FoodName,
                        ImgPath = FoodImgPath,
                        Price = FoodPrice,
                    };

                    var updateFoodReuslt = FoodDao.Instance.UpdateFood(UpdatedFood);
                    if (updateFoodReuslt)
                    {
                        MessageBox.Show("Update food successfully!", "Notification", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show("Update food fail!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }


                }
                else
                {


                    var maxFoodId = FoodDao.Instance.GetFoodIdMax();

                    var addFoodReuslt = FoodDao.Instance.AddFood(maxFoodId, FoodName, SelectedCategory.CategoryId, FoodPrice, FoodImgPath);
                    if (addFoodReuslt)
                    {
                        MessageBox.Show("Add food successfully!", "Notification", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show("Add food fail!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }

                }
                thisWindow.Close();

            });

            UploadFoodImageCommand = new RelayCommand<object>((p) => {
                return true;

            }, (p) => {
                // Create OpenFileDialog
                OpenFileDialog dlg = new OpenFileDialog();

                // Set filter for file extension and default file extension
                dlg.DefaultExt = ".png";
                dlg.Filter = "All files (*.*)|*.*|Image files (*.png;*.jpeg)|*.png;*.jpeg";

                // Display OpenFileDialog by calling ShowDialog method
                Nullable<bool> result = dlg.ShowDialog();

                if (result == true)
                {
                    // Open document
                    string filepath = dlg.FileName; // Stores Original Path in Textbox
                    string name = System.IO.Path.GetFileName(filepath);
                    string destinationPath = FileUtil.GetDestinationPath(name, "Images\\Foods");

                    File.Copy(filepath, destinationPath, true);

                    FoodImgPath = destinationPath;


                }

            });
        }
    }
}
