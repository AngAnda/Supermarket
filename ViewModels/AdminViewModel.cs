using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System.Windows.Input;

namespace Supermarket.ViewModels
{
    public class AdminViewModel : BaseViewModel
    {
        public ICommand UserCommand { get; set; }
        public ICommand CashierCommand { get; set; }
        public ICommand ProductCommand { get; set; }

        public ICommand StockCommand { get; set; }

        public ICommand CategoryCommand { get; set; }

        public ICommand BillCommand { get; set; }

        public ICommand ProducersCommand { get; set; }

        public AdminViewModel()
        {
            UserCommand = new RelayCommand(() => Messenger.Default.Send(new NotificationMessage("Users")));
            CashierCommand = new RelayCommand(() => Messenger.Default.Send(new NotificationMessage("Cashiers")));
            ProductCommand = new RelayCommand(() => Messenger.Default.Send(new NotificationMessage("Products")));
            StockCommand = new RelayCommand(() => Messenger.Default.Send(new NotificationMessage("Stocks")));
            CategoryCommand = new RelayCommand(() => Messenger.Default.Send(new NotificationMessage("Categories")));
            BillCommand = new RelayCommand(() => Messenger.Default.Send(new NotificationMessage("Bills")));
            ProducersCommand = new RelayCommand(() => Messenger.Default.Send(new NotificationMessage("Producers")));
        }

    }
}
