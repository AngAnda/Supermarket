using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Supermarket.Business;
using Supermarket.DataAccess;
using System;
using System.Collections.ObjectModel;

namespace Supermarket.ViewModels
{
    public class CashierViewModel : BaseViewModel
    {
        private readonly ProductService productService;
        private readonly CategoryService categoryService;
        private readonly ProducersService producersService;

        private ObservableCollection<Product> products;
        private ObservableCollection<Product> filteredProducts;
        private ObservableCollection<Producer> producers;
        private ObservableCollection<Category> categories;
        private ObservableCollection<string> barcodes;

        private int selectedProduct;
        private int selectedCategory;
        private int selectedProducer;
        private string selectedBarcode;
        private DateTime expirationDate;
        private int quantity;

        public CashierViewModel()
        {
            productService = new ProductService();
            categoryService = new CategoryService();
            producersService = new ProducersService();

            Products = productService.GetAll();
            Producers = producersService.GetAll();
            Categories = categoryService.GetAll();
            Barcodes = productService.GetBarcodes();
            FilteredProducts = productService.GetFilteredProducts(SelectedProduct, SelectedCategory, SelectedProducer, ExpirationDate);

            GoBackCommand = new RelayCommand(() => Messenger.Default.Send(new NotificationMessage("Login")));
        }

        public RelayCommand GoBackCommand { get; private set; }

        public ObservableCollection<Product> Products
        {
            get => products;
            set
            {
                products = value;
                OnPropertyChanged(nameof(Products));
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

        public ObservableCollection<Category> Categories
        {
            get => categories;
            set
            {
                categories = value;
                OnPropertyChanged(nameof(Categories));
            }
        }
        public ObservableCollection<string> Barcodes
        {
            get => barcodes;
            set
            {
                barcodes = value;
                OnPropertyChanged(nameof(Barcodes));
            }
        }

        public ObservableCollection<Product> FilteredProducts
        {
            get => filteredProducts;
            set
            {
                filteredProducts = value;
                OnPropertyChanged(nameof(FilteredProducts));
            }
        }
        public int SelectedProduct
        {
            get => selectedProduct;
            set
            {
                selectedProduct = value;
                OnPropertyChanged(nameof(SelectedProduct));
                FilteredProducts = productService.GetFilteredProducts(SelectedProduct, SelectedCategory, SelectedProducer, ExpirationDate);

            }
        }

        public int SelectedCategory
        {
            get => selectedCategory;
            set
            {
                selectedCategory = value;
                OnPropertyChanged(nameof(SelectedCategory));
                FilteredProducts = productService.GetFilteredProducts(SelectedProduct, SelectedCategory, SelectedProducer, ExpirationDate);

            }
        }

        public int SelectedProducer
        {
            get => selectedProducer;
            set
            {
                selectedProducer = value;
                OnPropertyChanged(nameof(SelectedProducer));
                FilteredProducts = productService.GetFilteredProducts(SelectedProduct, SelectedCategory, SelectedProducer, ExpirationDate);

            }
        }

        public string SelectedBarcode
        {
            get => selectedBarcode;
            set
            {
                selectedBarcode = value;
                OnPropertyChanged(nameof(SelectedBarcode));
                FilteredProducts = productService.GetFilteredProducts(SelectedProduct, SelectedCategory, SelectedProducer, ExpirationDate);

            }
        }


        public int Quantity
        {
            get => quantity;
            set
            {
                quantity = value;
                OnPropertyChanged(nameof(Quantity));
            }
        }

        public DateTime ExpirationDate
        {
            get => expirationDate;
            set
            {
                expirationDate = value;
                OnPropertyChanged(nameof(ExpirationDate));
                FilteredProducts = productService.GetFilteredProducts(SelectedProduct, SelectedCategory, SelectedProducer, ExpirationDate);

            }
        }
    }
}

