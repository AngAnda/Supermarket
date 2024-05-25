using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Supermarket.Business;
using System.Collections.ObjectModel;
using System.Linq;

namespace Supermarket.ViewModels
{
    public class CategoriesValueViewModel : BaseViewModel
    {

        public class CategoryValue
        {
            public string Name { get; set; }
            public decimal Value { get; set; }

            public CategoryValue(string name, decimal value)
            {
                Name = name;
                Value = value;
            }
        }

        private ObservableCollection<CategoryValue> categories;
        private readonly CategoryService _categoryService;

        public ObservableCollection<CategoryValue> Categories
        {
            get => categories;
            set
            {
                categories = value;
                OnPropertyChanged(nameof(Categories));
            }
        }

        public RelayCommand GoBackCommand { get; private set; }

        public CategoriesValueViewModel()
        {
            _categoryService = new CategoryService();

            var tupleList = _categoryService.GetTotalValues();
            Categories = new ObservableCollection<CategoryValue>(
               tupleList.Select(t => new CategoryValue(t.Name, t.Value))
            );

            GoBackCommand = new RelayCommand(() => Messenger.Default.Send(new NotificationMessage("Categories")));
        }

    }
}
