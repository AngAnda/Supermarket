using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Supermarket.Business;
using Supermarket.DataAccess;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Supermarket.ViewModels
{
    public class ProductProducerViewModel : BaseViewModel
    {
        private readonly ProducersService _producersService;
        private readonly ProductService _productService;
        private int _producerId;
        private ICommand GoBackCommand;

        public ObservableCollection<Product> products;
        private ObservableCollection<Producer> producers;

        public ObservableCollection<Product> Products
        {
            get => products;
            set
            {
                products = value;
                OnPropertyChanged(nameof(Products));
            }
        }

        public int ProducerId
        {
            get => _producerId;
            set
            {
                _producerId = value;
                OnPropertyChanged(nameof(ProducerId));

                Products = _productService.GetByProducer(ProducerId);
            }
        }



        public ObservableCollection<Producer> Producers
        {
            get => producers;
            set
            {
                producers = value;
                OnPropertyChanged(nameof(Producers));
            }
        }

        public ProductProducerViewModel()
        {
            _producersService = new ProducersService();
            _productService = new ProductService();

            Producers = _producersService.GetAll();
            Products = _productService.GetByProducer();
            Products = _productService.GetAll();
            GoBackCommand = new RelayCommand(() => { Messenger.Default.Send(new NotificationMessage("Producer")); });
        }
    }
}
