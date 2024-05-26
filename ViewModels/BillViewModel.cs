using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Supermarket.Business;
using Supermarket.DataAccess;
using System.Collections.ObjectModel;

namespace Supermarket.ViewModels
{
    public class BillViewModel : BaseViewModel
    {
        private ObservableCollection<Bill> _bills;
        private ObservableCollection<BillProduct> _billProducts;
        private readonly BillService _billService;
        private readonly BillProductService _billProductsService;
        private Bill _selectedBill;

        public RelayCommand GoBackCommand { get; private set; }

        public BillViewModel()
        {
            _billService = new BillService();
            _bills = new ObservableCollection<Bill>();
            _billProductsService = new BillProductService();

            GoBackCommand = new RelayCommand(() => Messenger.Default.Send(new NotificationMessage("Admin")));
            Bills = _billService.GetAll();
            //BillProducts = _billProductsService.GetAll();
        }

        public Bill SelectedBill
        {
            get => _selectedBill;
            set
            {
                if (_selectedBill != value)
                {
                    _selectedBill = value;
                    OnPropertyChanged(nameof(SelectedBill));
                }
            }
        }

        public ObservableCollection<Bill> Bills
        {
            get => _bills;
            set
            {
                _bills = value;
                OnPropertyChanged(nameof(Bills));
            }
        }

        public ObservableCollection<BillProduct> BillProducts
        {
            get => _billProducts;
            set
            {
                _billProducts = value;
                OnPropertyChanged(nameof(BillProducts));
            }
        }


    }
}