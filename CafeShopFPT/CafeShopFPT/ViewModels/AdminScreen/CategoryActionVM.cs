using CafeShopFPT.DAO.CategoryDao;
using CafeShopFPT.DAO.FoodDao;
using ProductCURD01.ViewModel;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;

namespace CafeShopFPT.ViewModels.AdminScreen
{
    public class CategoryActionVM : BaseVM
    {

        #region Property
        private string categoryId;

        private ObservableCollection<FoodDTO> _currentCategoryFoodList;
        public ObservableCollection<FoodDTO> CurrentCategoryFoodList
        {
            get
            {
                return _currentCategoryFoodList;
            }
            set
            {
                _currentCategoryFoodList = value; OnPropertyChanged();
            }
        }

        private ObservableCollection<FoodDTO> _selectCategoryFoodList;
        public ObservableCollection<FoodDTO> SelectCategoryFoodList
        {
            get
            {
                return _selectCategoryFoodList;
            }
            set
            {
                _selectCategoryFoodList = value; OnPropertyChanged();
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
                if (SelectedCategory != null)
                {
                    LoadSelectFoodData(SelectedCategory.CategoryId);
                }

            }
        }




        private string _title;
        public string Title
        {
            get
            {
                return _title;
            }
            set
            {
                _title = value;
            }
        }



        private string _selectedCategoryName;
        public string SelectedCategoryName
        {
            get
            {
                return _selectedCategoryName;
            }
            set
            {
                _selectedCategoryName = value;
            }
        }
        #endregion

        #region Function
        private void LoadCategoriesData()
        {
            var list = CategoryDao.Instance.LoadAllCategories();
            var category = list.Where(x => x.CategoryId.Equals(this.categoryId)).FirstOrDefault();
            if (category != null)
            {
                SelectedCategoryName = category.Name;
            }
            CategoriesList = new ObservableCollection<CategoryDTO>(list.Where(x => !x.CategoryId.Equals(categoryId)));
        }

        private void LoadCurrentFoodData()
        {
            CurrentCategoryFoodList = new ObservableCollection<FoodDTO>(FoodDao.Instance.LoadAllFoodByCategoryId(this.categoryId));

        }

        private void LoadSelectFoodData(string selectCategoryId)
        {
            SelectCategoryFoodList = new ObservableCollection<FoodDTO>(FoodDao.Instance.LoadAllFoodByCategoryId(selectCategoryId));

        }
        #endregion

        #region Command
        public ICommand AddCommand
        {
            get; set;
        }
        public ICommand RemoveCommand
        {
            get; set;
        }
        #endregion


        public CategoryActionVM(string categoryId)
        {
            this.categoryId = categoryId;
            LoadCategoriesData();
            LoadCurrentFoodData();



            AddCommand = new RelayCommand<object>((p) => {
                if (SelectedCategory != null)
                {
                    return true;
                }
                return false;

            }, (p) => {
                ListView listView = (ListView)p;
                System.Collections.IList items = (System.Collections.IList)listView.SelectedItems;
                var collection = items.Cast<FoodDTO>();


                foreach (var food in collection)
                {
                    FoodDao.Instance.UpdateFoodCategory(this.categoryId, food.FoodId);
                }
                LoadCurrentFoodData();
                LoadSelectFoodData(SelectedCategory.CategoryId);

            });

            RemoveCommand = new RelayCommand<object>((p) => {
                if (SelectedCategory != null)
                {
                    return true;
                }
                return false;

            }, (p) => {
                ListView listView = (ListView)p;
                System.Collections.IList items = (System.Collections.IList)listView.SelectedItems;
                var collection = items.Cast<FoodDTO>();

                foreach (var food in collection)
                {
                    FoodDao.Instance.UpdateFoodCategory(SelectedCategory.CategoryId, food.FoodId);
                }
                LoadCurrentFoodData();
                LoadSelectFoodData(SelectedCategory.CategoryId);



            });
        }
    }
}
