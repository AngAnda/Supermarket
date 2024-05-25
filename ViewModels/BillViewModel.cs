using GalaSoft.MvvmLight.Command;
using Supermarket.Business;
using Supermarket.DataAccess;
using System.Collections.ObjectModel;

namespace Supermarket.ViewModels
{
    public class BillViewModel : BaseViewModel
    {
        private ObservableCollection<Bill> _bills;
        private ObservableCollection<BillProduct> _billProducts;
        private readonly BillService _billService; // Serviciu presupus pentru încărcarea datelor

        public BillViewModel()
        {
            _billService = new BillService();
            Bills = _billService.GetAll();
            //BillProducts = _billService.GetAllBillProduct();
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


        public RelayCommand GoBackCommand { get; private set; }
    }
}