using GalaSoft.MvvmLight.Messaging;
using System.Windows.Input;

namespace SupermarketManagementSystem.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private BaseViewModel _currentViewModel;

        public MainViewModel(LoginViewModel loginViewModel, AdminViewModel adminViewModel, CashierViewModel cashierViewModel)
        {
            CurrentViewModel = loginViewModel;
            ShowAdminCommand = new RelayCommand<object>(o => CurrentViewModel = adminViewModel);
            ShowCashierCommand = new RelayCommand<object>(o => CurrentViewModel = cashierViewModel);
            Messenger.Default.Register<NotificationMessage>(this, OnReceivedMessage);

        }

        private void OnReceivedMessage(NotificationMessage message)
        {
            if (message.Notification == "Admin")
            {
                CurrentViewModel = new AdminViewModel();
            }
            else if (message.Notification == "Cashier")
            {
                CurrentViewModel = new CashierViewModel();
            }
        }

        public BaseViewModel CurrentViewModel
        {
            get => _currentViewModel;
            set
            {
                _currentViewModel = value;
                OnPropertyChanged(nameof(CurrentViewModel));
            }
        }

        public ICommand ShowAdminCommand { get; set; }
        public ICommand ShowCashierCommand { get; set; }
    }
}
