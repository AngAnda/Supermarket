using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Supermarket.Business;
using Supermarket.DataAccess;
using System;
using System.Collections.ObjectModel;
using System.Windows;

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
                AddStockCommand.RaiseCanExecuteChanged();
                EditStockCommand.RaiseCanExecuteChanged();
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
                    DeleteStockCommand.RaiseCanExecuteChanged();
                    EditStockCommand.RaiseCanExecuteChanged();
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
                    AddStockCommand.RaiseCanExecuteChanged();
                    EditStockCommand.RaiseCanExecuteChanged();
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
                    AddStockCommand.RaiseCanExecuteChanged();
                    EditStockCommand.RaiseCanExecuteChanged();
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
                    AddStockCommand.RaiseCanExecuteChanged();
                    EditStockCommand.RaiseCanExecuteChanged();
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
                    AddStockCommand.RaiseCanExecuteChanged();
                    EditStockCommand.RaiseCanExecuteChanged();
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
                    AddStockCommand.RaiseCanExecuteChanged();
                    EditStockCommand.RaiseCanExecuteChanged();
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
                    AddStockCommand.RaiseCanExecuteChanged();
                    EditStockCommand.RaiseCanExecuteChanged();
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
                    AddStockCommand.RaiseCanExecuteChanged();
                    EditStockCommand.RaiseCanExecuteChanged();
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
                AddStockCommand.RaiseCanExecuteChanged();
                EditStockCommand.RaiseCanExecuteChanged();
            }
        }
        public RelayCommand AddStockCommand { get; private set; }
        public RelayCommand EditStockCommand { get; private set; }
        public RelayCommand DeleteStockCommand { get; private set; }
        public RelayCommand GoBackCommand { get; private set; }

        public RelayCommand RefreshFieldsCommand { get; private set; }

        public StockViewModel()
        {
            _stockService = new StockService();
            _productService = new ProductService();
            RegisterCommands();
            Stocks = _stockService.GetAll();
            Products = _productService.GetAll();
            StockExpirationDate = DateTime.Now;
            StockSupplyDate = DateTime.Now;
        }



        private void RegisterCommands()
        {
            AddStockCommand = new RelayCommand(AddStock, CandAdd);
            EditStockCommand = new RelayCommand(EditStock, CanEdit);
            DeleteStockCommand = new RelayCommand(DeleteStock, CanDelete);
            RefreshFieldsCommand = new RelayCommand(RefreshFields);
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
                SelectedStock.StockSellingPrice = StockSellingPrice;
                SelectedStock.ProductId = ProductId;

                try
                {
                    _stockService.Update(SelectedStock);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
                Stocks = _stockService.GetAll();
            }
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

        private bool CandAdd()
        {
            if (_productId.Equals(0)
                || _stockQuantity.Equals(0)
                || _stockPurchasePrice.Equals(0)
                || _stockUnitOfMeasure.Equals(""))
                return false;
            return true;
        }

        private bool CanDelete()
        {
            if (SelectedStock == null)
                return false;
            return true;
        }

        private bool CanEdit()
        {
            return CandAdd() && CanDelete();
        }
    }
}