using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Supermarket.Business;
using Supermarket.DataAccess;
using System;
using System.Collections.ObjectModel;

namespace Supermarket.ViewModels
{
    public class StockViewModel : BaseViewModel
    {
        private readonly StockService _stockService;
        private readonly ProductService _productService;

        private ObservableCollection<Stock> _stocks;
        public ObservableCollection<Stock> Stocks
        {
            get => _stocks;
            set
            {
                _stocks = value;
                OnPropertyChanged(nameof(Stocks));
            }
        }

        private Stock _selectedStock;
        public Stock SelectedStock
        {
            get => _selectedStock;
            set
            {
                _selectedStock = value;
                OnPropertyChanged(nameof(SelectedStock));
                if (value != null)
                {
                    StockQuantity = value.StockQuantity;
                    StockUnitOfMeasure = value.StockUnitOfMeasure;
                    StockSupplyDate = value.StockSupplyDate;
                    StockExpirationDate = value.StockExpirationDate;
                    StockPurchasePrice = value.StockPurchasePrice;
                    StockSellingPrice = value.StockSellingPrice;
                    ProductId = value.ProductId;
                }
            }
        }

        private int _stockQuantity;
        private string _stockUnitOfMeasure;
        private DateTime _stockSupplyDate;
        private DateTime _stockExpirationDate;
        private decimal _stockPurchasePrice;
        private decimal _stockSellingPrice;
        private int _productId;

        public int StockQuantity
        {
            get => _stockQuantity;
            set
            {
                if (_stockQuantity != value)
                {
                    _stockQuantity = value;
                    OnPropertyChanged(nameof(StockQuantity));
                }
            }
        }

        public string StockUnitOfMeasure
        {
            get => _stockUnitOfMeasure;
            set
            {
                if (_stockUnitOfMeasure != value)
                {
                    _stockUnitOfMeasure = value;
                    OnPropertyChanged(nameof(StockUnitOfMeasure));
                }
            }
        }

        public DateTime StockSupplyDate
        {
            get => _stockSupplyDate;
            set
            {
                if (_stockSupplyDate != value)
                {
                    _stockSupplyDate = value;
                    OnPropertyChanged(nameof(StockSupplyDate));
                }
            }
        }

        public DateTime StockExpirationDate
        {
            get => _stockExpirationDate;
            set
            {
                if (_stockExpirationDate != value)
                {
                    _stockExpirationDate = value;
                    OnPropertyChanged(nameof(StockExpirationDate));
                }
            }
        }

        public decimal StockPurchasePrice
        {
            get => _stockPurchasePrice;
            set
            {
                if (_stockPurchasePrice != value)
                {
                    _stockPurchasePrice = value;
                    OnPropertyChanged(nameof(StockPurchasePrice));
                }
            }
        }

        public decimal StockSellingPrice
        {
            get => _stockSellingPrice;
            set
            {
                if (_stockSellingPrice != value)
                {
                    _stockSellingPrice = value;
                    OnPropertyChanged(nameof(StockSellingPrice));
                }
            }
        }

        public int ProductId
        {
            get => _productId;
            set
            {
                if (_productId != value)
                {
                    _productId = value;
                    OnPropertyChanged(nameof(ProductId));
                }
            }
        }

        public ObservableCollection<Product> products;

        public ObservableCollection<Product> Products
        {
            get => products;
            set
            {
                products = value;
                OnPropertyChanged(nameof(Products));
            }
        }
        public RelayCommand AddStockCommand { get; private set; }
        public RelayCommand EditStockCommand { get; private set; }
        public RelayCommand DeleteStockCommand { get; private set; }
        public RelayCommand GoBackCommand { get; private set; }

        public RelayCommand RelayCommand { get; private set; }

        public StockViewModel()
        {
            _stockService = new StockService();
            _productService = new ProductService();
            Stocks = _stockService.GetAll();
            Products = _productService.GetAll();
            RegisterCommands();
        }



        private void RegisterCommands()
        {
            AddStockCommand = new RelayCommand(AddStock);
            EditStockCommand = new RelayCommand(EditStock, CanEditOrDelete);
            DeleteStockCommand = new RelayCommand(DeleteStock, CanEditOrDelete);
            RelayCommand = new RelayCommand(RefreshFields);
            GoBackCommand = new RelayCommand(() => Messenger.Default.Send(new NotificationMessage("Admin")));
        }

        private void AddStock()
        {
            var newStock = new Stock
            {
                StockQuantity = StockQuantity,
                StockUnitOfMeasure = StockUnitOfMeasure,
                StockSupplyDate = StockSupplyDate,
                StockExpirationDate = StockExpirationDate,
                StockPurchasePrice = StockPurchasePrice,
                StockSellingPrice = StockSellingPrice,
                ProductId = ProductId
            };
            _stockService.Add(newStock);
            Stocks.Add(newStock);

        }

        private void EditStock()
        {
            if (SelectedStock != null)
            {
                SelectedStock.StockQuantity = StockQuantity;
                SelectedStock.StockUnitOfMeasure = StockUnitOfMeasure;
                SelectedStock.StockSupplyDate = StockSupplyDate;
                SelectedStock.StockExpirationDate = StockExpirationDate;
                SelectedStock.StockPurchasePrice = StockPurchasePrice;
                SelectedStock.StockSellingPrice = StockSellingPrice;
                SelectedStock.ProductId = ProductId;

                _stockService.Update(SelectedStock);
                Stocks = _stockService.GetAll();
            }
        }

        private bool CanEditOrDelete()
        {
            return true;
        }

        private void DeleteStock()
        {
            if (SelectedStock != null)
            {
                _stockService.Delete(SelectedStock.StockId);
                Stocks = _stockService.GetAll();
            }
        }

        private void RefreshFields()
        {
            StockQuantity = 0;
            StockUnitOfMeasure = "";
            StockSupplyDate = DateTime.Now;
            StockExpirationDate = DateTime.Now;
            StockPurchasePrice = 0;
            StockSellingPrice = 0;
            ProductId = 0;
        }
    }
}