using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Supermarket.Business;
using Supermarket.DataAccess;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Supermarket.ViewModels
{
    public class CategoryViewModel : BaseViewModel
    {
        private readonly CategoryService categoryService;
        private ObservableCollection<Category> _categories;
        private Category _selectedCategory;
        private string _name;

        public ObservableCollection<Category> Categories
        {
            get => _categories;
            set
            {
                _categories = value;
                OnPropertyChanged(nameof(Categories));
            }
        }

        public Category SelectedCategory
        {
            get => _selectedCategory;
            set
            {
                _selectedCategory = value;
                OnPropertyChanged(nameof(SelectedCategory));
            }
        }

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        public ICommand AddCategoryCommand { get; private set; }
        public ICommand EditCategoryCommand { get; private set; }
        public ICommand DeleteCategoryCommand { get; private set; }
        public ICommand GoBackCommand { get; private set; }
        public ICommand RefreshCommand { get; private set; }

        public CategoryViewModel()
        {
            categoryService = new CategoryService();
            Categories = categoryService.GetAll();
            AddCategoryCommand = new RelayCommand(AddCategory);
            EditCategoryCommand = new RelayCommand(EditCategory, CanEditOrDelete);
            DeleteCategoryCommand = new RelayCommand(DeleteCategory, CanEditOrDelete);
            GoBackCommand = new RelayCommand(GoBack);
            RefreshCommand = new RelayCommand(() => Name = string.Empty);
        }

        private void AddCategory()
        {
            if (!string.IsNullOrWhiteSpace(Name))
            {
                var newCategory = new Category
                {
                    CategoryName = Name,
                };
                categoryService.Add(newCategory);
                SelectedCategory = newCategory;
                Name = string.Empty;
                Categories = categoryService.GetAll();
            }
        }

        private void EditCategory()
        {
            var newCategory = new Category
            {
                CategoryId = SelectedCategory.CategoryId,
                CategoryName = Name,
            };
            categoryService.Edit(newCategory);
            SelectedCategory = newCategory;
            Name = string.Empty;
            Categories = categoryService.GetAll();
        }

        private void DeleteCategory()
        {
            categoryService.Delete(SelectedCategory.CategoryId);
            SelectedCategory = null;
            Categories = categoryService.GetAll();

        }

        private bool CanEditOrDelete()
        {
            //return SelectedCategory != null;
            return true;
        }

        public void GoBack()
        {
            Messenger.Default.Send(new NotificationMessage("Admin"));
        }
    }
}