using GalaSoft.MvvmLight.Messaging;

namespace Supermarket.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private BaseViewModel _currentViewModel;

        public BaseViewModel CurrentViewModel
        {
            get => _currentViewModel;
            set
            {
                _currentViewModel = value;
                OnPropertyChanged(nameof(CurrentViewModel));
            }
        }

        public MainViewModel()
        {
            CurrentViewModel = new LoginViewModel();
            //CurrentViewModel = new UserValueViewModel();
            Messenger.Default.Register<NotificationMessage>(this, OnReceivedMessage);

        }

        private void OnReceivedMessage(NotificationMessage message)
        {
            string notification = message.Notification;
            switch (notification)
            {
                case "Users":
                    CurrentViewModel = new UsersViewModel();
                    break;
                case "Cashiers":
                    CurrentViewModel = new CashierViewModel(); // de modificat
                    break;
                case "Products":
                    CurrentViewModel = new ProductViewModel();
                    break;
                case "Stocks":
                    CurrentViewModel = new StockViewModel();
                    break;
                case "Categories":
                    CurrentViewModel = new CategoryViewModel();
                    break;
                case "Bills":
                    CurrentViewModel = new BillViewModel();
                    break;
                case "Admin":
                    CurrentViewModel = new AdminViewModel();
                    break;
                case "Cashier":
                    CurrentViewModel = new CashierViewModel();
                    break;
                case "Producers":
                    CurrentViewModel = new ProducerViewModel();
                    break;
                case "ProductProducers":
                    CurrentViewModel = new ProductProducerViewModel();
                    break;
                case "ListCategoriesValue":
                    CurrentViewModel = new CategoriesValueViewModel();
                    break;
                case "UserValue":
                    CurrentViewModel = new UserValueViewModel();
                    break;
            }
        }
    }
}
