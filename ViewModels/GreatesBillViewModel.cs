using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Supermarket.Business;
using Supermarket.DataAccess;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace Supermarket.ViewModels
{
    public class GreatesBillViewModel : BaseViewModel
    {
        private DateTime selectedDate;
        private ObservableCollection<Bill> selectedBill;
        private ObservableCollection<BillProduct> selectedBillProducts;
        private readonly BillService billService;
        private readonly BillProductService billProductService;

        public RelayCommand GoBackCommand { get; private set; }

        public GreatesBillViewModel()
        {
            billService = new BillService();
            billProductService = new BillProductService();
            SelectedDate = DateTime.Now;
            GoBackCommand = new RelayCommand(() => Messenger.Default.Send(new NotificationMessage("Bills")));
        }

        public DateTime SelectedDate

        {
            get => selectedDate;
            set
            {
                selectedDate = value;
                OnPropertyChanged(nameof(SelectedDate));
                LoadBillData();
            }
        }

        public ObservableCollection<Bill> SelectedBill
        {
            get => selectedBill;
            set
            {
                selectedBill = value;
                OnPropertyChanged(nameof(SelectedBill));
            }
        }

        public ObservableCollection<BillProduct> SelectedBillProduct
        {
            get => selectedBillProducts;
            set
            {
                selectedBillProducts = value;
                OnPropertyChanged(nameof(SelectedBillProduct));
            }
        }

        private void LoadBillData()
        {
            SelectedBill = billService.GetGreatestBill(selectedDate);
            if (SelectedBill.Any())
                SelectedBillProduct = billProductService.GetById(SelectedBill.First().BillId);
        }


    }
}
